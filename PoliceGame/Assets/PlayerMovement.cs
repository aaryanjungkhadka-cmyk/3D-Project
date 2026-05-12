using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public Animator anim; // Drag your character model here in the Inspector
    
    private Rigidbody rb;
    private float distToGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        // Make sure you have a Capsule Collider on the player!
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        // Jump Logic
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // UPDATE ANIMATOR HERE
        if (anim != null)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // This checks if you are pressing any movement keys
            bool isMoving = (horizontal != 0 || vertical != 0);

            // This tells your 'iswalking' parameter to turn ON or OFF
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

        Vector3 move = (transform.forward * v + transform.right * h).normalized;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}