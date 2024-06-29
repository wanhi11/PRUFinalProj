using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    public GameObject panelQuestion;
    public TMP_Text questionText;
    public TMP_Text[] answerTexts;
    public Button[] answerButtons;
    [SerializeField] private int value;

    private int correctAnswerIndex;
    private Vector3 playerStartPosition;
    private ItemManager itemManager;
    private PlayerMovement playerMovement;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Assuming the player has a tag "Player" and its initial position is the respawn position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStartPosition = player.transform.position;
            playerMovement = player.GetComponent<PlayerMovement>(); // Get the PlayerMovement component
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }

        // Item manager
        itemManager = ItemManager.Instance;
        if (itemManager == null)
        {
            Debug.LogError("ItemManager instance not found.");
        }
    }

    public void ShowQuestion(string question, string[] answers, int correctAnswer, GameObject gameObject)
    {
        if (panelQuestion != null)
        {
            panelQuestion.SetActive(true);
            questionText.text = question;
            correctAnswerIndex = correctAnswer;

            // Disable player movement
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            for (int i = 0; i < answerTexts.Length; i++)
            {
                if (i < answers.Length)
                {
                    answerTexts[i].text = answers[i];
                    answerButtons[i].gameObject.SetActive(true);
                    int answerIndex = i; // Capture the index for the lambda closure
                    answerButtons[i].onClick.RemoveAllListeners(); // Remove previous listeners
                    answerButtons[i].onClick.AddListener(() => OnClickAnswer(answerIndex, gameObject));
                }
                else
                {
                    answerButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("PanelQuestion GameObject not assigned.");
        }
    }

    public void OnClickAnswer(int answerIndex, GameObject gameObject)
    {
        if (answerIndex == correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            if (gameObject != null)
            {
                Destroy(gameObject);
            }

            // Add reward value to the item manager
            if (itemManager != null)
            {
                itemManager.ChangeItems(value);
            }
        }
        else
        {
            Debug.Log("Wrong answer!");
            // Respawn the player at the start position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerStartPosition;
            }

            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }

        // Re-enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        panelQuestion.SetActive(false); // Hide the panel after an answer is selected
    }
}
