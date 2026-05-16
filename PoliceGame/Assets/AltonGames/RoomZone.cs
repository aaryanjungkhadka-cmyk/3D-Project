using UnityEngine;

public class RoomZone : MonoBehaviour
{
    public static bool playerInsideRoom = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInsideRoom = true;
            Debug.Log("PLAYER ENTERED ROOM");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInsideRoom = false;
            Debug.Log("PLAYER LEFT ROOM");
        }
    }
}