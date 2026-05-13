using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 5f;
    public float height = 3f;

    public float mouseSensitivity = 100f;
    
    // NEW: Add a LayerMask so the camera knows what a "Wall" is
    public LayerMask wallLayers; 

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
        xRotation = Mathf.Clamp(xRotation, -10f, 70f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate the rotation and the "Ideal" position (where camera wants to be)
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 offset = rotation * Vector3.forward * distance;
        Vector3 idealPosition = target.position - offset + Vector3.up * height;

        // 2. Raycast Logic: Check if there is a wall between the cop and the idealPosition
        RaycastHit hit;
        Vector3 direction = idealPosition - target.position;

        // We shoot a ray from the cop's head towards the camera's desired spot
        if (Physics.Raycast(target.position + Vector3.up * height, direction, out hit, direction.magnitude, wallLayers))
        {
            // If we hit a wall, move the camera to the hit point (and slightly away from the wall)
            transform.position = hit.point + hit.normal * 0.2f;
        }
        else
        {
            // No wall? Use the ideal position
            transform.position = idealPosition;
        }

        // 3. Look at the cop (shifted up slightly so we look at his head, not his feet)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}