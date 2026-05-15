using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [Header("Settings")]
    public float openAngle = 90f; 
    public float speed = 5f;

    void Start()
    {
        // Force the rotation to 0,0,0 at the very start
        transform.localRotation = Quaternion.identity;
        
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        
        isOpen = false;
    }

    void Update()
    {
        Quaternion target = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * speed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}