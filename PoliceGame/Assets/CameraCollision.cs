using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target;          // Drag 'cop' here
    public Vector3 offset = new Vector3(0, 2, -4); // Position behind the cop
    public float smoothSpeed = 10f;
    public float rotationSmoothSpeed = 10f;
    public LayerMask obstacleLayers;  // Set to 'Default' or 'Everything'

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate the 'Ideal' position based on the Cop's CURRENT rotation
        // This makes the camera rotate when the cop rotates
        Vector3 desiredPosition = target.TransformPoint(offset);
        
        // 2. Collision Check (The "Wall Fix")
        Vector3 direction = desiredPosition - target.position;
        if (Physics.Raycast(target.position + Vector3.up * 1.5f, direction.normalized, out RaycastHit hit, direction.magnitude, obstacleLayers))
        {
            desiredPosition = hit.point + hit.normal * 0.2f;
        }

        // 3. Smoothly move the camera to that position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // 4. Rotate the camera to face the same way the cop is facing
        Quaternion targetRotation = Quaternion.LookRotation((target.position + Vector3.up * 1.5f) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
    }
}