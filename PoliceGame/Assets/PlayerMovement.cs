using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 20f;
    public float jumpForce = 12f;
    public float rotationSpeed = 15f;
    
    [Header("Interaction Settings")]
    public float interactDistance = 2.5f; 
    public KeyCode interactKey = KeyCode.E; 

    [Header("References")]
    public Animator anim;
    
    private Rigidbody rb;
    private Transform camTransform;
    private Vector3 moveInput;
    private bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (Camera.main != null) camTransform = Camera.main.transform;

        // --- NEW FIX START ---
        SnapToSurface();
        // --- NEW FIX END ---
    }

    // This function automatically places the character on the ground at start
    void SnapToSurface()
    {
        RaycastHit hit;
        // Shoots a ray from way above the player down to find the ground
        if (Physics.Raycast(transform.position + Vector3.up * 50f, Vector3.down, out hit, 100f))
        {
            // Move player to the hit point + a small offset so they aren't stuck IN the floor
            transform.position = hit.point + Vector3.up * 0.1f;
        }
        
        // Forces the character to stand upright (no leaning)
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }

        if (anim != null)
        {
            anim.SetBool("iswalking", moveInput.magnitude > 0.1f);
        }
    }

    void TryInteract()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.2f;
        
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, interactDistance))
        {
            // Fixed: Search the hit object or its parents for the DoorController
            DoorController door = hit.collider.GetComponentInParent<DoorController>();
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
    }

    void FixedUpdate()
    {
        if (camTransform == null) return;

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y = 0f;
        right.y = 0f;
        
        Vector3 processedMove = (forward.normalized * moveInput.z + right.normalized * moveInput.x).normalized;

        if (processedMove.magnitude > 0.1f)
        {
            Vector3 targetVelocity = processedMove * speed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

            Quaternion targetRotation = Quaternion.LookRotation(processedMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    bool IsGrounded()
    {
        // Using a slightly longer ray for grounded check to account for gravity
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f);
    }
}