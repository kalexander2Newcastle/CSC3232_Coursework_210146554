using UnityEngine;

public class TeleportToHub : MonoBehaviour
{
    [SerializeField] private string targetTag = "HubSpawn";
    [SerializeField] private GameObject star;

    /// <summary>
    /// The OnCollisionEnter method teleports the player to the game object with the tag "HubSpawn" and disables the star
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameObject hubSpawn = GameObject.FindGameObjectWithTag(targetTag);

            if (hubSpawn != null)
            {
                Vector3 previousPosition = collision.transform.position;

                collision.transform.position = hubSpawn.transform.position;

                Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                }

                if (star != null)
                {
                    star.SetActive(false);
                }
            }
        }
    }
}
