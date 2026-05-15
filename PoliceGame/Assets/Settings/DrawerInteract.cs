using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 targetPosition;

    [Header("Settings")]
    public Vector3 openOffset = new Vector3(0, 0, 0.5f); 
    public float speed = 5f;

    void Start()
    {
        closedPosition = transform.localPosition;
        targetPosition = closedPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public void ToggleDrawer()
    {
        isOpen = !isOpen;
        targetPosition = isOpen ? closedPosition + openOffset : closedPosition;
    }
}