using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    public ParticleSystem splashEffect;

    /// <summary>
    /// The OnTriggerEnter method checks if the player collides with the water, emiting particles every time they touch the object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (splashEffect != null)
            {
                splashEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                splashEffect.Play();
            }
        }
    }
}
