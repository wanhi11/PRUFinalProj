using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishValue : MonoBehaviour
{
    private FinishGame finishGame;

    void Start()
    {
        finishGame = FindObjectOfType<FinishGame>(); // Find the FinishGame in the scene
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishGame.FinishValue(); // Call FinishGame when the player touches the finish trigger
        }
    }
}
