using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target; // The target to look at, e.g., the center of your plane
    public float sensitivity = 5f; // Sensitivity for camera movement
    public float minAngle = -89f; // Minimum pitch angle
    public float maxAngle = -20f; // Maximum pitch angle
    public float rotationSpeed = 50f; // Speed of camera rotation around the target

    private float currentAngle = -45f; // Start angle for pitch
    private float currentRotation = 0f; // Start angle for rotation around the target

    void Start() {
        // Adjust camera's viewport rect to shift the scene upwards and leave space at the bottom
        GetComponent<Camera>().rect = new Rect(0, 0.15f, 1, 0.85f); // Leaves 15% of the screen at the bottom
    }

    void Update() {
        // Get input from the up and down arrow keys
        float verticalInput = Input.GetAxis("Vertical");
        currentAngle -= verticalInput * sensitivity * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // Get input from the left and right arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");
        currentRotation += horizontalInput * -rotationSpeed * Time.deltaTime;

        // Calculate the new position and rotation
        Quaternion pitchRotation = Quaternion.Euler(currentAngle, 0, 0);
        Quaternion yawRotation = Quaternion.Euler(0, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -12f); // Distance from the target

        transform.position = target.position - yawRotation * pitchRotation * direction;
        transform.LookAt(target); // Always look at the target
    }
}
