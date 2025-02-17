using UnityEngine;
using System.Collections;

public class PlayerHurt : MonoBehaviour
{
    private Vector3 startPosition;
    private float respawnDelay = 1.5f;
    private PlayerMovement playerMovement;
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsMaterial2D originalMaterial;
    private bool isTouchingDoor;

    void Start()
    {
        startPosition = transform.position;

        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on the player object.");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found on the player object.");
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }
        else
        {
            originalMaterial = rb.sharedMaterial;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Monster"))
        {
            ApplyKnockback(collision.gameObject.transform.position.x);
            StartCoroutine(RespawnAfterDelay());
        }
        else if (collision.gameObject.CompareTag("Door"))
        {
            HandleDoorCollision();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            ApplyKnockback(other.transform.position.x);
            StartCoroutine(RespawnAfterDelay());
        }
        else if (other.CompareTag("Door"))
        {
            HandleDoorCollision();
        }
    }

    void ApplyKnockback(float otherPositionX)
    {
        rb.sharedMaterial = null; // Disable friction for knockback

        float knockbackForce = 10f; // Adjust this value as needed
        if (otherPositionX > transform.position.x)
        {
            rb.velocity = new Vector2(-knockbackForce, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(knockbackForce, rb.velocity.y);
        }
    }

    void HandleDoorCollision()
    {
        isTouchingDoor = true;

        // Stop player movement and disable animations
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (anim != null)
        {
            anim.SetTrigger("idle"); // Ensure there's an "idle" trigger in the Animator
        }

        // Optionally, you might want to reset the velocity
        rb.velocity = Vector2.zero;
    }

    IEnumerator RespawnAfterDelay()
    {
        if (anim != null)
        {
            anim.SetTrigger("hurt");
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        yield return new WaitForSeconds(respawnDelay);

        transform.position = startPosition;

        rb.sharedMaterial = originalMaterial;

        if (playerMovement != null && !isTouchingDoor)
        {
            playerMovement.enabled = true;
        }
    }
}
