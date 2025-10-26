using UnityEngine;

/// <summary>
/// This script manages the rotation of a stationary camera.
/// The rotation is controlled by mouse movement near the screen edges
/// and by the W, A, S, D keys for vertical and horizontal rotation.
/// </summary>
public class EdgeAndKeyCameraRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The speed at which the camera rotates when the mouse is at the screen edges.")]
    public float edgeRotationSpeed = 30f;

    [Tooltip("The speed at which the camera rotates using the WASD keys (in degrees per second).")]
    public float keyRotationSpeed = 50f;

    [Tooltip("The width in pixels from the screen edge to trigger rotation.")]
    public float edgeBoundary = 50f;

    [Header("Vertical Rotation Limits (Pitch)")]
    [Tooltip("The maximum upward rotation angle.")]
    public float maxPitch = 80f;

    [Tooltip("The maximum downward rotation angle.")]
    public float minPitch = -80f;

    // Private variables to handle rotation angles
    private float yaw = 0f;   // Horizontal rotation (around Y axis)
    private float pitch = 0f; // Vertical rotation (around X axis)
    [SerializeField] CanvasGroup chat;
    void Start()
    {
        // Initialize the rotation angles with the camera’s initial rotation
        // to avoid jumps at startup.
        Vector3 initialAngles = transform.eulerAngles;
        yaw = initialAngles.y;
        pitch = initialAngles.x;
    }

    void Update()
    {
        // --- Keyboard Input Handling (WASD) ---
        float horizontalInput = 0f;
        float verticalInput = 0f;
        if (chat.alpha == 1)
        {
            return;
        }
        // Horizontal rotation with A and D
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        // Vertical rotation with W and S
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        // Apply keyboard rotation
        yaw += horizontalInput * keyRotationSpeed * Time.deltaTime;
        pitch -= verticalInput * keyRotationSpeed * Time.deltaTime; // Pitch is inverted for a more natural control



        /* EDGE ROTATION WITH MOUSE
         * 
        // --- Mouse Input Handling at Screen Edges ---
        Vector3 mousePosition = Input.mousePosition;

        // Horizontal rotation with mouse
        if (mousePosition.x < edgeBoundary)
        {
            yaw -= edgeRotationSpeed * Time.deltaTime;
        }
        else if (mousePosition.x > Screen.width - edgeBoundary)
        {
            yaw += edgeRotationSpeed * Time.deltaTime;
        }

        // Vertical rotation with mouse
        if (mousePosition.y < edgeBoundary)
        {
            pitch += edgeRotationSpeed * Time.deltaTime; // Inverted compared to keyboard
        }
        else if (mousePosition.y > Screen.height - edgeBoundary)
        {
            pitch -= edgeRotationSpeed * Time.deltaTime; // Inverted compared to keyboard
        }

         */

        // --- Applying and Limiting Rotation ---

        // Limit the vertical rotation (pitch) to prevent the camera from flipping over itself.
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Apply the final rotation to the camera.
        // Quaternion.Euler converts Euler angles (yaw, pitch) into a rotation that Unity can use.
        // The Z axis (roll) is set to 0 to prevent side tilting.
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
