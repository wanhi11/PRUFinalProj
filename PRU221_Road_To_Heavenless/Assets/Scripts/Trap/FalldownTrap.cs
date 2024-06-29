using System.Collections;
using UnityEngine;

public class Falldown : MonoBehaviour
{
    public float fallDelay; // Delay before the platform starts to fall
    public float respawnDelay; // Delay before the platform respawns

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Initially, the platform is not affected by physics
        initialPosition = transform.position; // Store the initial position of the platform
        col = GetComponent<Collider2D>(); // Get the Collider2D component
        col.isTrigger = false; // Ensure it is not a trigger initially
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding with the platform is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the fall coroutine
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(fallDelay);

        // Enable physics so that the platform falls
        rb.isKinematic = false;

        // Switch to trigger mode so that the platform does not collide with other objects while falling
        col.isTrigger = true;

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Respawn the platform
        Respawn();
    }

    void Respawn()
    {
        // Disable physics again
        rb.isKinematic = true;
        col.isTrigger = false;

        // Reset the platform's position to the initial position
        transform.position = initialPosition;

        // Reset the platform's velocity
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}