using UnityEngine;
using System.Collections;

public class DrawerInteract : MonoBehaviour
{
    private bool isFlying = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    [Header("Flying Settings")]
    public Vector3 flyOffset = new Vector3(0, 2f, 3f);
    public Vector3 flyRotation = new Vector3(-45, 90, 0);
    public float speed = 5f;

    [Header("Evidence")]
    public bool containsEvidence = true;

    [Header("Win Card")]
    public GameObject winCard;

    private bool evidenceTriggered = false;

    private Vector3 targetPos;
    private Quaternion targetRot;

    void Start()
    {
        // Save starting transform
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        targetPos = startPosition;
        targetRot = startRotation;

        // Hide win card at start
        if (winCard != null)
        {
            winCard.SetActive(false);
        }
    }

    void Update()
    {
        // Smooth movement
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * speed
        );

        // Smooth rotation
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRot,
            Time.deltaTime * speed
        );

        // Press E to open/close drawer
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleDrawer();
        }

        // Press ENTER to instantly show win card
        if (evidenceTriggered && Input.GetKeyDown(KeyCode.Return))
        {
            ShowWinCard();
        }
    }

    public void ToggleDrawer()
    {
        isFlying = !isFlying;

        if (isFlying)
        {
            // Move drawer outward/upward
            targetPos = startPosition + flyOffset;

            // Rotate drawer
            targetRot = Quaternion.Euler(flyRotation);

            // Trigger evidence jump
            if (containsEvidence && !evidenceTriggered)
            {
                EvidenceJump evidence = GetComponentInChildren<EvidenceJump>();

                if (evidence != null)
                {
                    evidence.StartJumping();

                    evidenceTriggered = true;

                    // Start 3-second timer
                    StartCoroutine(WinDelay());
                }
            }
        }
        else
        {
            // Return to original position
            targetPos = startPosition;
            targetRot = startRotation;
        }
    }

    IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(3f);

        ShowWinCard();
    }

    void ShowWinCard()
    {
        if (winCard != null)
        {
            winCard.SetActive(true);
        }
    }
}