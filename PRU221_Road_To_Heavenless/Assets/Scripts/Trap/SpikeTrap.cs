using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float amplitude = 1.0f; // The depth of the movement.
    public float frequency = 1.0f; // The frequency of the movement.
    public float speed; // The speed multiplier of the movement, with a default value.

    private Vector3 startPosition;

    void Start()
    {
        // Store the starting position of the GameObject.
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate the new Y position based on the sine wave formula, ensuring it only moves down.
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency * speed) * amplitude * -0.5f + amplitude * -0.5f;

        // Apply the new position to the GameObject.
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}