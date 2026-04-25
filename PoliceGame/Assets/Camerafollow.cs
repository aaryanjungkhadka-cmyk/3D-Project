using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 5f;
    public float height = 3f;

    public float mouseSensitivity = 100f;

    float xRotation = 20f;
    float yRotation = 0f;

    void Update()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        // Limit up/down looking
        xRotation = Mathf.Clamp(xRotation, -30f, 70f);
    }

    void LateUpdate()
    {
        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Position camera behind player
        Vector3 position = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        transform.position = position;
        transform.LookAt(target);
    }
}