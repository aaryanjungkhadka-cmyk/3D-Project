using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Transform player;

    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        closedPos = transform.position;

        openPos = closedPos + new Vector3(2f, 0f, 0f);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < 5f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                openPos,
                Time.deltaTime * 2
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                closedPos,
                Time.deltaTime * 2
            );
        }
    }
}