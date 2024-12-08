using UnityEngine;

public class WindVectorField : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(1f, 0f, 0f); 
    public float windStrength = 5f; 
    public float effectRadius = 10f;

    /// <summary>
    /// The OnTriggerStay method applies a upward wind force to objects within the radius
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 windForce = windDirection.normalized * windStrength;
                rb.AddForce(windForce, ForceMode.Force);
            }
    }
}
