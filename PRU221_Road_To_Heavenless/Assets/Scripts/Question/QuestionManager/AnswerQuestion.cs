using UnityEngine;

public class AnswerQuestion : MonoBehaviour
{
    public string question;
    public string[] answers;
    public int correctAnswer; // Changed to string to match ShowQuestion parameter
    public bool statusQuestion = false;
    [SerializeField] public GameObject Enemy;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (QuestionManager.Instance != null)
            {
                QuestionManager.Instance.ShowQuestion(question, answers, correctAnswer, Enemy);
            }
            else
            {
                Debug.LogError("QuestionManager instance is null.");
            }
        }
    }
}

