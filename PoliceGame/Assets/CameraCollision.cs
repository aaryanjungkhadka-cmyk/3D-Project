using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target;          
    public Vector3 offset = new Vector3(0, 2, -4); 
    public Vector3 crouchOffset = new Vector3(0, 1.2f, -3); // Lower and closer
    public float smoothTime = 0.15f;    
    public LayerMask obstacleLayers;  
    
    [Header("Look Down Settings")]
    public float crouchSpeed = 5f;
    private float crouchAmount = 0f; // 0 = standing, 1 = sitting

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentDesiredPos;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Handle Input for 'Q'
        bool isCrouching = Input.GetKey(KeyCode.Q);
        
        // Smoothly interpolate between 0 and 1
        float targetCrouch = isCrouching ? 1f : 0f;
        crouchAmount = Mathf.Lerp(crouchAmount, targetCrouch, Time.deltaTime * crouchSpeed);

        // 2. Interpolate the Offset and the Look-At height
        Vector3 activeOffset = Vector3.Lerp(offset, crouchOffset, crouchAmount);
        float lookHeight = Mathf.Lerp(1.5f, 0.8f, crouchAmount); // Look lower when sitting

        // 3. Calculate the ideal position behind the cop
        Vector3 worldOffset = target.TransformPoint(activeOffset);
        
        // 4. Collision Check: rayStart moves down based on lookHeight
        Vector3 rayStart = target.position + Vector3.up * lookHeight;
        Vector3 direction = worldOffset - rayStart;
        float distance = direction.magnitude;

        if (Physics.Raycast(rayStart, direction.normalized, out RaycastHit hit, distance, obstacleLayers))
        {
            currentDesiredPos = hit.point + hit.normal * 0.2f;
        }
        else
        {
            currentDesiredPos = worldOffset;
        }

        // 5. Apply Movement
        transform.position = Vector3.SmoothDamp(transform.position, currentDesiredPos, ref currentVelocity, smoothTime);

        // 6. Look at the adjusted height
        Quaternion targetRotation = Quaternion.LookRotation(rayStart - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
    }
}