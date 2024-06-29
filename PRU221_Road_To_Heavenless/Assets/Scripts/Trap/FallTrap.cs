using UnityEngine;
using System.Collections;

public class FallTrap : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Ensure the Rigidbody2D is initially kinematic
        rb.gravityScale = 0; // Disable gravity initially
        originalPosition = transform.position; // Store the original position
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isFalling) // Use CompareTag for better performance
        {
            rb.isKinematic = false; // Make Rigidbody2D dynamic to fall down
            rb.gravityScale = 1; // Ensure gravity scale is set to 1
            isFalling = true;
            StartCoroutine(Respawn());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !rb.isKinematic) // Only trigger animation if the spikehead is dynamic and touches the player
        {
            Animator playerAnimator = col.gameObject.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Hurt"); // Trigger the Hurt animation
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds

        // Reset the position and make Rigidbody2D kinematic again
        transform.position = originalPosition;
        rb.velocity = Vector2.zero; // Stop any movement
        rb.isKinematic = true; // Make Rigidbody2D kinematic again
        rb.gravityScale = 0; // Disable gravity
        isFalling = false; // Reset the falling flag
    }
}