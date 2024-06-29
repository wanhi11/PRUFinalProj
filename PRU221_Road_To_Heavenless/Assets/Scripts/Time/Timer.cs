using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    private bool isRunning;

    void Start()
    {
        // Reset the timer when the scene starts
        if (SceneController.Timer != 0f)
        {
            currentTime = SceneController.Timer;
        }
        else
        {
            currentTime = 0f;
        }

        if (timerText != null)
        {
            timerText.text = currentTime.ToString("F2");
        }
        isRunning = true;
    }

    private void OnEnable()
    {
        // Initialize currentTime from PlayerPrefs if it's still 0 after Start
        if (currentTime == 0)
        {
            currentTime = PlayerPrefs.GetFloat("time", 0f); // Default to 0 if no saved value
        }
        if (timerText != null)
        {
            timerText.text = currentTime.ToString("F2");
        }
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            // Update the timer based on countDown flag
            if (countDown)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime += Time.deltaTime;
            }

            // Format the time to display two decimal places
            if (timerText != null)
            {
                timerText.text = currentTime.ToString("F2");
            }

            // Save the current time to PlayerPrefs
            PlayerPrefs.SetFloat("time", currentTime);
            SceneController.Timer = currentTime;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = currentTime.ToString("F2");
        }
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        if (timerText != null)
        {
            timerText.text = currentTime.ToString("F2");
        }
        PlayerPrefs.SetFloat("time", currentTime);
        SceneController.Timer = currentTime;
        isRunning = true;
    }
}