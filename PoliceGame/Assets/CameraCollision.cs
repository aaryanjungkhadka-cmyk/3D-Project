using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;          
    public Vector3 offset = new Vector3(0.8f, 1.0f, -5.0f); 
    
    [Header("Mouse Settings")]
    public float sensitivity = 2.5f;
    public float minVerticalAngle = -20f; 
    public float maxVerticalAngle = 60f;  
    
    [Header("Zoom Settings")]
    public float zoomSpeed = 8f;
    public float minDistance = 2f;
    public float maxDistance = 15f; 
    public float zoomSmoothTime = 0.1f;

    [Header("Smoothing & Collision")]
    public float moveSmoothTime = 0.05f; 
    public LayerMask obstacleLayers;  

    private float currentHorizontalAngle = 0f;
    private float currentVerticalAngle = 20f; 
    private float targetZoomDistance;
    private float currentZoomDistance;
    private float zoomVelocity = 0f;
    private Vector3 moveVelocity = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
        
        targetZoomDistance = offset.magnitude;
        currentZoomDistance = targetZoomDistance;
        Cursor.lockState = CursorLockMode.Locked;

        // --- NEW: INSTANT SNAP ON START ---
        if (target != null)
        {
            Vector3 pivotPoint = target.position + Vector3.up * 1.6f;
            Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
            Vector3 direction = rotation * offset.normalized;
            transform.position = pivotPoint + (direction * currentZoomDistance);
            transform.rotation = rotation;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. INPUT
        currentHorizontalAngle += Input.GetAxis("Mouse X") * sensitivity;
        currentVerticalAngle -= Input.GetAxis("Mouse Y") * sensitivity;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

        // 2. SMOOTH ZOOM
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetZoomDistance -= scroll * zoomSpeed;
        targetZoomDistance = Mathf.Clamp(targetZoomDistance, minDistance, maxDistance);
        currentZoomDistance = Mathf.SmoothDamp(currentZoomDistance, targetZoomDistance, ref zoomVelocity, zoomSmoothTime);

        // 3. CALCULATE TARGET POSITION
        Vector3 pivotPoint = target.position + Vector3.up * 1.6f;
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
        
        Vector3 direction = rotation * offset.normalized;
        Vector3 desiredPos = pivotPoint + (direction * currentZoomDistance);

        // 4. COLLISION
        if (Physics.Raycast(pivotPoint, (desiredPos - pivotPoint).normalized, out RaycastHit hit, currentZoomDistance, obstacleLayers))
        {
            desiredPos = hit.point + hit.normal * 0.2f;
        }

        // 5. APPLY POSITION & ROTATION
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref moveVelocity, moveSmoothTime);
        transform.rotation = rotation; 
    }
}