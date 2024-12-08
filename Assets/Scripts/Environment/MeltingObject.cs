using UnityEngine;

public class IceCubeMelt : MonoBehaviour
{
    public Transform waterPlane;
    public float meltTime = 180f;  
    private float meltRate;        
    private Rigidbody rb;
    private bool isInWater = false;

    /// <summary>
    /// The Start method gets the object's rigidbody and sets the melting rate of the ice cube according to its starting size
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meltRate = transform.localScale.x / meltTime;
    }

    /// <summary>
    /// The OnTriggerEnter method checks if the object collides with the trigger volume with the "Water" tag, to assign 'isInWater'
    /// to true if the object touches water
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    /// <summary>
    /// The OnTriggerExit method checks if the object isn't colliding with the trigger volume with the "Water" tag to assign 'isInWater'
    /// to false if the object is not touching water
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }

    /// <summary>
    /// The FixedUpdate method checks if the object is either in water or underwater before calling the MeltIceCube method
    /// </summary>
    void FixedUpdate()
    {
        if (isInWater || IsUnderwater())
        {
            MeltIceCube();
        }
    }

    /// <summary>
    /// The IsUnderwater method checks if the object is below the water level
    /// </summary>
    /// <returns></returns>
    bool IsUnderwater()
    {
        float waterLevel = waterPlane.position.y;
        return transform.position.y < waterLevel;
    }

    /// <summary>
    /// The MeltIceCube method shrinks the size of the object, when the object shrinks to below the minimum size, it gets destroyed
    /// </summary>
    void MeltIceCube()
    {
        if (transform.localScale.x > 0.1f)
        {
            float shrinkAmount = meltRate * Time.deltaTime;
            transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Ice cube fully melted");
        }
    }
}
