using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    int maxHealth = 300;
    int currentHealth = 300;
    int damage = 100;
    int lives = 3;
    int stars = 0;
    int coins = 0;

    public float baseKnockbackForce = 1f;
    public float knockbackMultiplier = 1f;
    private Rigidbody playerRb;

    private bool knockbackApplied = false;
    public float knockbackResetDelay = 0.5f;
    private bool teleportedAfterLifeLost = false;

    public GameObject hubSpawnPoint;

    private bool disableKnockback = false;

    private HashSet<GameObject> defeatedEnemies = new HashSet<GameObject>();

    private bool isGrounded; 

    /// <summary>
    /// The start method gets the rigidbody of the player
    /// </summary>
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// The OnTriggerEnter method is triggered when the player enters a trigger collider (e.g., enemy head, enemy body,
    /// cannonball, coin, star)
    /// Coroutine is used to reset knockback
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (disableKnockback) return;

        // Handle collision with a star
        if (other.CompareTag("Star"))
        {
            stars++;
            Debug.Log("Stars Collected: " + stars);
        }

        // Handle collision with a coin
        if (other.CompareTag("Coin"))
        {
            coins++;
            Debug.Log("Coins Collected: " + coins);
        }

        // Check if the player collides with the enemy's head (trigger)
        if (other.CompareTag("EnemyHead"))
        {
            GameObject enemy = other.transform.root.gameObject;
            Debug.Log("Player collided with Enemy Head. Disabling the specific enemy.");
            enemy.SetActive(false);
        }

        // Handle collision with the enemy body (trigger)
        if (other.CompareTag("EnemyBody") && !knockbackApplied)
        {
            currentHealth -= damage;
            Debug.Log("Current Health: " + currentHealth);

            Rigidbody enemyRb = other.GetComponentInParent<Rigidbody>();

            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            knockbackDirection.y = 0; 
            knockbackDirection = knockbackDirection.normalized;

            float forceMultiplier = enemyRb != null ? (enemyRb.mass / playerRb.mass) : 1;
            playerRb.AddForce(knockbackDirection * baseKnockbackForce * forceMultiplier, ForceMode.Impulse);

            knockbackApplied = true;

            StartCoroutine(ResetKnockback());

            CheckPlayerDeath();
        }

        // Handle collision with a cannonball
        if (other.CompareTag("Cannonball") && !knockbackApplied)
        {
            knockbackApplied = true;

            int healthReduction = maxHealth / 3;
            currentHealth -= healthReduction;
            Debug.Log("Health reduced by 1/3. Current Health: " + currentHealth);

            Rigidbody cannonballRb = other.GetComponent<Rigidbody>();

            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            knockbackDirection.y = 0;
            knockbackDirection = knockbackDirection.normalized;

            float forceMultiplier = cannonballRb != null ? (cannonballRb.mass / playerRb.mass) : 1;
            playerRb.AddForce(knockbackDirection * baseKnockbackForce * forceMultiplier, ForceMode.Impulse);

            StartCoroutine(ResetKnockback());

            knockbackResetDelay = 0;

            CheckPlayerDeath();
        }
    }

    /// <summary>
    /// Coroutine to reset the knockbackApplied flag after a delay
    /// </summary>
    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackResetDelay);
        knockbackApplied = false;
    }

    /// <summary>
    /// The CheckPlayerDeath method checks how much health and lives the player has left, and whether or not to
    /// teleport the player back to spawn or hub
    /// </summary>
    private void CheckPlayerDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            lives--;
            Debug.Log("Lives remaining: " + lives);

            teleportedAfterLifeLost = false;

            if (!teleportedAfterLifeLost)
            {
                TeleportToSpawnPoint();
                teleportedAfterLifeLost = true;
            }

            if (lives <= 0)
            {
                Debug.Log("Game Over!");
                lives = 3;
                currentHealth = maxHealth;

                if (hubSpawnPoint != null)
                {
                    transform.position = hubSpawnPoint.transform.position;
                }
                else
                {
                    Debug.LogError("No spawn point");
                }
            }
        }
    }

    /// <summary>
    /// The TeleportToSpawnPoint method teleports the player to the spawn point when called
    /// </summary>
    private void TeleportToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogError("No spawn point");
        }
    }
}
