using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float reachDistance = 2.5f; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // This creates a point 1.5 units in front of the cop
            Vector3 interactionPoint = transform.position + (Vector3.up * 1.0f) + (transform.forward * 1.5f);
            
            // VISUAL DEBUG: Creates a yellow sphere in Scene view so you see the "hit zone"
            Debug.Log("Checking for drawers at: " + interactionPoint);

            // This looks for ANY collider in a 1-meter bubble around that point
            Collider[] hitColliders = Physics.OverlapSphere(interactionPoint, 1.0f);
            
            bool foundDrawer = false;
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Bubble touched: " + hitCollider.gameObject.name);
                
                if (hitCollider.TryGetComponent(out DrawerInteract drawer))
                {
                    drawer.ToggleDrawer();
                    Debug.Log("<color=green>SUCCESS: Drawer opened via Bubble!</color>");
                    foundDrawer = true;
                    break;
                }
            }

            if (!foundDrawer) Debug.Log("<color=red>Bubble missed everything.</color>");
        }
    }

    // This lets you see the bubble in the Scene view while NOT playing
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 interactionPoint = transform.position + (Vector3.up * 1.0f) + (transform.forward * 1.5f);
        Gizmos.DrawWireSphere(interactionPoint, 1.0f);
    }
}