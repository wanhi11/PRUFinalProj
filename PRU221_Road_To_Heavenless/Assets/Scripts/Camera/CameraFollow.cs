using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Speed at which the camera transitions between rooms
    [SerializeField] private float speed;
    private float currentPosX; // Stores the X position of the new room for the camera to move to
    private float currentPosY; // Stores the Y position of the new room for the camera to move to
    private Vector3 velocity = Vector3.zero; // Used for smooth dampening of the camera movement

    // Variables for following the player
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private float aheadDistance; // Distance ahead of the player the camera should look
    [SerializeField] private float cameraSpeed; // Speed at which the camera adjusts its position
    private float lookAhead; // Variable to store the look-ahead distance based on player's scale and aheadDistance

    // Update is called once per frame
    private void Update()
    {
        // Update the camera's position to follow the player on the X,Y axis, while maintaining its Y and Z positions
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

        // Smoothly interpolate the lookAhead value based on the player's scale and aheadDistance
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.y), Time.deltaTime * cameraSpeed);
    }

    // Method to move the camera to a new room
    public void MoveToNewRoom(Transform _newRoom)
    {
        // Update currentPosX with the X,Y position of the new room
        currentPosX = _newRoom.position.x;
        currentPosY = _newRoom.position.y;
    }
}