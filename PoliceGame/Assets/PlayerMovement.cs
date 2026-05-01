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
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Update Animator speed parameter
        if (anim != null)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // Calculate how fast we are moving (0 to 1)
            float moveMagnitude = new Vector2(horizontal, vertical).magnitude;
            anim.SetFloat("Speed", moveMagnitude);
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