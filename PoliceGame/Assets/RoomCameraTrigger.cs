using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform mainCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCamera.position = cameraPosition.position;
            mainCamera.rotation = cameraPosition.rotation;
        }
    }
}