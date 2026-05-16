using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    public GameObject winCard;

    private void Start()
    {
        // Hide win card at start
        if (winCard != null)
        {
            winCard.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the knife: " + other.name);

        // Detect player
        if (other.CompareTag("Player") || other.name == "Police")
        {
            Debug.Log("Player found the knife!");

            if (winCard != null)
            {
                winCard.SetActive(true);

                // Optional freeze
                Time.timeScale = 0f;

                Debug.Log("Win card shown.");
            }
            else
            {
                Debug.LogError("Win Card is NOT assigned in Inspector!");
            }
        }
    }
}