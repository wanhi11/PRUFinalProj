using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed; // Speed of the monster's movement
    public float distance; // Distance to move left and right
    public bool moveRight = true; // Initial movement direction

    private Vector3 startingPosition;

    void Start()
    {
        // Save the initial position of the monster
        startingPosition = transform.position;
    }

    void Update()
    {
        MoveMonster();
    }

    void MoveMonster()
    {
        // Determine the direction and move the monster accordingly
        if (moveRight)
        {
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);

            // Check if the monster has moved the specified distance to the right
            if (transform.position.x >= startingPosition.x + distance)
            {
                // Change direction to move left
                moveRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);

            // Check if the monster has moved the specified distance to the left
            if (transform.position.x <= startingPosition.x - distance)
            {
                // Change direction to move right
                moveRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        // Multiply the monster's x local scale by -1 to flip it
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}