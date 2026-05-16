using UnityEngine;

public class Room001Trigger : MonoBehaviour
{
    public GameUIManager ui;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.ShowBloodRoomCard();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.HideBloodRoomCard();
        }
    }
}