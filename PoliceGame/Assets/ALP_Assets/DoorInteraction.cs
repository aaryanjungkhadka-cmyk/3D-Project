using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float reachDistance = 10.0f;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // Detect Tap or Click
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            Ray ray = mainCam.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, reachDistance))
            {
                // This script ONLY looks for doors
                if (hit.collider.TryGetComponent(out DoorInteract door))
                {
                    door.ToggleDoor();
                    Debug.Log("<color=cyan>Door Interaction Success!</color>");
                }
            }
        }
    }
}