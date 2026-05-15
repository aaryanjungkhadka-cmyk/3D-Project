using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 8f;
    public float height = 4f;

    public float mouseSensitivity = 100f;

    [Header("Wall Detection")]
    public LayerMask wallLayers;

    float xRotation = 20f;
    float yRotation = 0f;

    void Update()
    {
        // Mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        // Limit vertical rotation
        xRotation = Mathf.Clamp(xRotation, -10f, 70f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Camera rotation
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Camera position behind player
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // Desired position
        Vector3 idealPosition = target.position + Vector3.up * height + offset;

        // Raycast to stop camera going through walls
        RaycastHit hit;

        Vector3 rayStart = target.position + Vector3.up * height;
        Vector3 direction = idealPosition - rayStart;

        if (Physics.Raycast(rayStart, direction.normalized, out hit, distance, wallLayers))
        {
            // Move camera slightly forward from wall
            transform.position = hit.point + hit.normal * 0.3f;
        }
        else
        {
            transform.position = idealPosition;
        }

        // Look slightly above player center
        transform.LookAt(target.position + Vector3.up * 2.5f);
    }
}