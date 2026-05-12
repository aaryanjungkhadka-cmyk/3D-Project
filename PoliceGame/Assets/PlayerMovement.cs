using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 1. INCREASED SPEED
    public float speed = 50f; 
    public float jumpForce = 8f;
    public float rotationSpeed = 10f; // How fast the body turns
    public Animator anim; 
    
    private Rigidbody rb;
    private float distToGround;
    private Transform camTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        distToGround = GetComponent<Collider>().bounds.extents.y;
        
        // Get the camera reference
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (anim != null)
        {
            bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
            anim.SetBool("iswalking", isMoving);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. CALCULATE DIRECTION RELATIVE TO CAMERA
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        // Keep movement on the flat ground (ignore camera's up/down tilt)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * v + right * h).normalized;

        // 3. MOVE THE CHARACTER
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

        // 4. ROTATE BODY TO FACE MOVEMENT DIRECTION
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            // Smoothly rotate the body toward the new direction
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}