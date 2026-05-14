using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f; 
    public float jumpForce = 12f;
    public float rotationSpeed = 15f; 
    
    [Header("References")]
    public Animator anim; 
    
    private Rigidbody rb;
    private Transform camTransform;
    private Vector3 moveInput;
    private bool jumpRequested; // New flag for safe physics jumping

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Essential physics settings to prevent jitter
        rb.freezeRotation = true; 
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (Camera.main != null) camTransform = Camera.main.transform;
    }

    void Update()
    {
        // 1. Get Input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // Store movement input
        moveInput = new Vector3(h, 0, v);

        // 2. Capture Jump Input in Update (Standard practice)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequested = true;
        }

        // 3. Animation Sync
        if (anim != null)
        {
            anim.SetBool("iswalking", moveInput.magnitude > 0.1f);
        }
    }

    void FixedUpdate()
    {
        // 4. Calculate camera-relative movement INSIDE FixedUpdate
        // This ensures the camera position used matches the physics step
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y = 0f;
        right.y = 0f;
        
        Vector3 processedMove = (forward.normalized * moveInput.z + right.normalized * moveInput.x).normalized;

        // 5. Movement Logic
        if (processedMove.magnitude > 0.1f)
        {
            Vector3 targetVelocity = processedMove * speed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

            // Smooth Rotation
            Quaternion targetRotation = Quaternion.LookRotation(processedMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // STOP FIX
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        // 6. Apply Jump in FixedUpdate
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false; // Reset flag
        }
    }

    bool IsGrounded()
    {
        // Raycast logic from image_bfa4f8.png
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 1.5f);
    }
}