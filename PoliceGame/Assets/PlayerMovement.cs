using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S

        Vector3 move = new Vector3(moveX, 0, moveZ);

        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}

