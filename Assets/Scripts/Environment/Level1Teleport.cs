using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public string spawnPointTag = "SpawnPoint";

    /// <summary>
    /// The OnTriggerEnter method checks if the object colliding with the door is the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TeleportPlayerToSpawn(collision.gameObject);
        }
    }

    /// <summary>
    /// The TeleportPlayerToSpawn method finds the spawn point and teleports the player to the spawn
    /// </summary>
    /// <param name="player"></param>
    private void TeleportPlayerToSpawn(GameObject player)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag(spawnPointTag);

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogError("No spawn point set");
        }
    }
}
