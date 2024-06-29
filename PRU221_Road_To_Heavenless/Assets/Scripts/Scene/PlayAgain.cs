using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(1);
        ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetGame()
    {
        // Reset timer
        if (FindObjectOfType<Timer>() != null)
        {
            FindObjectOfType<Timer>().ResetTimer();
        }

        // Reset item values
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.ResetItems();
        }
    }
}
