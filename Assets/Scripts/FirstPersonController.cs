
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 3f;
    public float mouseSensitivity = 2f;
    public float floatHeight = 2f;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Look around with the mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);
        playerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);

        // Move with WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement =
            transform.right * x + transform.forward * z;

        controller.Move(movement * moveSpeed * Time.deltaTime);

        // Keep the player in the air  ig?
        Vector3 position = transform.position;
        position.y = floatHeight;
        transform.position = position;
    }
}
