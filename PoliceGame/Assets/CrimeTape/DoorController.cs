using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f; // How far the door opens
    public float smoothSpeed = 2f; // How fast it swings
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Remember the starting rotation (closed)
        closedRotation = transform.localRotation;
        targetRotation = closedRotation;
    }

    void Update()
    {
        // Smoothly rotate toward the target
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            // Open it
            targetRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        }
        else
        {
            // Close it
            targetRotation = closedRotation;
        }
    }
}