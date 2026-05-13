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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Essential physics settings for a character
        rb.freezeRotation = true; 
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (Camera.main != null) camTransform = Camera.main.transform;
    }

    void Update()
    {
        // 1. Get Input (Raw for instant stopping)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. Calculate direction relative to Camera
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y = 0f;
        right.y = 0f;
        moveInput = (forward.normalized * v + right.normalized * h).normalized;

        // 3. Jump Logic (Using the 1.5f fix for your high pivot point)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // Reset vertical velocity before jumping
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // 4. Animation Sync
        if (anim != null)
        {
            anim.SetBool("iswalking", moveInput.magnitude > 0.1f);
        }
    }

    void FixedUpdate()
    {
        // 5. Movement Logic
        if (moveInput.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveInput * speed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

            // Smooth Rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // STOP FIX: Kills the horizontal velocity immediately when keys are released
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    bool IsGrounded()
    {
        // PIVOT FIX: Based on your screenshot (image_ea4bfb.png), the pivot is in the stomach.
        // We shoot the ray 1.5 meters down to ensure it reaches the feet.
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 1.5f);
    }
}