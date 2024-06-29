using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Move the player horizontally
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip the player's sprite based on the direction of movement
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(5, 5, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-5, 5, 1);

        // Check for jump input and whether the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            Jump();

        // Set animator parameters
        anim.SetBool("run", Mathf.Abs(horizontalInput) > 0.01f);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("jump");
        grounded = false;
    }

    // Called when the player collides with another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            anim.SetTrigger("grounded");
        grounded = true;
    }

    // Called when the player stops colliding with another collider
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }
}