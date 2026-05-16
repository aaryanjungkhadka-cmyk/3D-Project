using UnityEngine;

public class EvidenceJump : MonoBehaviour
{
    private bool shouldJump = false;
    private Vector3 targetAirPosition;
    
    [Header("Settings")]
    public float jumpHeight = 0.5f; 
    public float jumpSpeed = 3f;
    public float rotationSpeed = 150f;

    void Update()
    {
        if (shouldJump)
        {
            // Move to the target position relative to where the jump started
            transform.position = Vector3.Lerp(transform.position, targetAirPosition, Time.deltaTime * jumpSpeed);
            
            // Spin it for effect
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void StartJumping()
    {
        // THIS IS THE FIX: Calculate the target ONLY when the drawer is already open
        targetAirPosition = transform.position + Vector3.up * jumpHeight;
        shouldJump = true;
    }
}