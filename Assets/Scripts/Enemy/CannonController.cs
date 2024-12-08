using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject cannonballPrefab; 
    public Transform shootPoint;       
    public Transform player;           
    public float launchSpeed = 15f;    
    public float fireInterval = 2f;    
    public float fireRange = 30f;      
    private float fireTimer;
    private int shotCounter = 0;

    /// <summary>
    /// The update method checks if the player is within range before firing the cannonball
    /// </summary>
    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval && IsPlayerInRange())
        {
            FireCannonball();
            fireTimer = 0;
        }
    }

    /// <summary>
    /// The FireCannonball method increments the shot counter for each shot, firing a larger and heavier cannonball 
    /// every 3rd shot. It adjusts the cannonball's mass and size accordingly. It also predicts the player's future 
    /// position to calculate the trajectory of the shot.
    /// </summary>
    void FireCannonball()
    {
        shotCounter++;

        bool isHeavyShot = shotCounter % 3 == 0;
        
        Vector3 futurePosition = PredictPlayerPosition(player);
        Vector3 velocity = CalculateLaunchVelocity(futurePosition, isHeavyShot);

        GameObject cannonball = Instantiate(cannonballPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        if (isHeavyShot)
        {
            float heavyCannonballMass = 15f; 
            float heavyCannonballSize = 1.5f; 

            rb.mass = heavyCannonballMass;
            cannonball.transform.localScale = new Vector3(heavyCannonballSize, heavyCannonballSize, heavyCannonballSize);
        }
        else
        {
            float normalCannonballMass = 10f; 
            float normalCannonballSize = 1f; 

            rb.mass = normalCannonballMass;
            cannonball.transform.localScale = new Vector3(normalCannonballSize, normalCannonballSize, normalCannonballSize);
        }
        rb.velocity = velocity;
    }

    /// <summary>
    /// The IsPlayerInRange method checks if the player is in range of the cannon
    /// </summary>
    /// <returns></returns>
    bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(player.position, shootPoint.position);
        return distanceToPlayer <= fireRange;
    }

    /// <summary>
    /// The PredictPlayerPosition method calculates the player's future position using 
    /// the player's current velocity and the time the cannonball would take to hit them
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    Vector3 PredictPlayerPosition(Transform player)
    {
        float timeToImpact = Vector3.Distance(player.position, shootPoint.position) / launchSpeed;
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        return player.position + (playerRb.velocity * timeToImpact);
    }

    /// <summary>
    /// The CalculateLaunchVelocity method calculates the velocity needed for the
    /// cannonball to hit the predicted position of the player, adjusting the velocity
    /// heavy cannonballs
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="isHeavyShot"></param>
    /// <returns></returns>
    Vector3 CalculateLaunchVelocity(Vector3 targetPosition, bool isHeavyShot)
    {
        Vector3 direction = targetPosition - shootPoint.position;

        float distance = direction.magnitude;
        float angle = 45f * Mathf.Deg2Rad;
        float velocityMagnitude = Mathf.Sqrt((distance * Mathf.Abs(Physics.gravity.y)) / Mathf.Sin(2 * angle));

        if (isHeavyShot)
        {
            velocityMagnitude *= 0.8f;
        }

        Vector3 velocity = direction.normalized * velocityMagnitude;
        velocity.y = velocityMagnitude * Mathf.Sin(angle);

        if (!isHeavyShot)
        {
            velocity.y *= 0.9f;
        }
        return velocity;
    }
}