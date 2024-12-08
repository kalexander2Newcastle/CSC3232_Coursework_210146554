using UnityEngine;

public class WaterBuoyancy : MonoBehaviour
{
    public Transform waterPlane;  
    public float buoyancyForce = 10f;
    private Rigidbody rb;
    private int waterContactCount = 0; 

    /// <summary>
    /// The start method gets the player's rigidbody
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// The OnTriggerEnter method incremenets the waterContactCount if entering a trigger with the "Water" tag
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterContactCount++;
        }
    }

    /// <summary>
    /// The OnTriggerExit method decrements the waterContactCount if exiting a trigger with the "Water" tag
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterContactCount = Mathf.Max(0, waterContactCount - 1);
        }
    }

    /// <summary>
    /// The FixedUpdate method sets buoyancy to only be applied if below the water plane and is in contact with water
    /// </summary>
    void FixedUpdate()
    {
        if (IsUnderwater() && waterContactCount > 0)
        {
            ApplyBuoyancy();
        }
    }

    /// <summary>
    /// The IsUnderwater method checks if the object is below the water surface using the water's y position
    /// </summary>
    /// <returns></returns>
    bool IsUnderwater()
    {
        float waterLevel = waterPlane.position.y;
        return transform.position.y < waterLevel;
    }

    /// <summary>
    /// The ApplyBuoyancy method applies buoyancy only when the player is below the water surface and increases the
    /// force of buoyancy depending on the depth of the player
    /// </summary>
    void ApplyBuoyancy()
    {
        float waterLevel = waterPlane.position.y;

        if (transform.position.y < waterLevel)
        {
            float depth = waterLevel - transform.position.y;

            if (depth > 0)
            {
                float forceMagnitude = buoyancyForce * depth;
                rb.AddForce(Vector3.up * forceMagnitude, ForceMode.Force);
            }
        }
    }
}
