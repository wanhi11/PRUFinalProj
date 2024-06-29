using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassScene : MonoBehaviour
{
    [SerializeField] private string sceneName; // The name of the scene to load when the player touches the sign
    public float delaySecond = 0;
    public string nameScene = "Level2";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Pass to the specified scene
            // SceneController.instance.NextLevel();
            collision.gameObject.SetActive(false);
            ModeSelect();
        }
    }

    public void ModeSelect()
    {
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delaySecond);
        SceneManager.LoadScene(2);
    }
}