using UnityEngine;

public class CannonballBehavior : MonoBehaviour
{
    /// <summary>
    /// The OnCollisionEnter method destroys the cannonball on impact
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); 
    }
}
