using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;   // Adjust this value to change the speed of the camera movement.

    void Update()
    {
        // Get the horizontal and vertical input values.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the new position of the camera based on the input values and the move speed.
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, verticalInput * moveSpeed * Time.deltaTime);

        // Set the camera's position to the new position.
        transform.position = newPosition;
    }
    
    public void OnSpeedChanged(float value)
    {
        moveSpeed = value;
    }
}
