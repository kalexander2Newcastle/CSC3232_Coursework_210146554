using UnityEngine;

public class DynamicPhysicMaterialHandler : MonoBehaviour
{
    [SerializeField] private PhysicMaterial enemyLandMaterial;
    [SerializeField] private PhysicMaterial waterMaterial;  
    [SerializeField] private PhysicMaterial defaultMaterial; 
    [SerializeField] private Transform waterPlane;    

    private CapsuleCollider playerCollider; 
    private int waterContactCount = 0;      
    private bool isTouchingEnemyLand = false; 

    /// <summary>
    /// The start method gets the player's collider at sets the player's physic material to the default one
    /// </summary>
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();

        if (defaultMaterial != null)
        {
            playerCollider.material = defaultMaterial;
        }
    }

    /// <summary>
    /// The OnTriggerEnter method checks to see if the player collides with objects with the "Water" tag, incrementing the
    /// waterContactCount if they do.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterContactCount++;
        }
        AssignMaterial();
    }

    /// <summary>
    /// The OnTriggerExit method checks to see if the player doesn't collide with objects with the "Water" tag, decrementing the
    /// waterContactCount if they do
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterContactCount--;
        }
        AssignMaterial();
    }

    /// <summary>
    /// The OnCollisionEnter method checks if the player is touching objects with the layer "EnemyLand", assigning the player
    /// the appropriate material
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyLand"))
        {
            isTouchingEnemyLand = true;
        }
        AssignMaterial();
    }

    /// <summary>
    /// The OnCollisionExit method checks if the player isn't touching objects with the layer "Enemyland", assigning the player
    /// the appropriate material
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionExit(Collision other)
    {
        Debug.Log($"OnCollisionExit with: {other.gameObject.name}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyLand"))
        {
            isTouchingEnemyLand = false;
            Debug.Log("Exited EnemyLand: isTouchingEnemyLand = false");
        }
        AssignMaterial();
    }

    /// <summary>
    /// The FixedUpdate method continuously applies the appropriate material 
    /// </summary>
    void FixedUpdate()
    {
        AssignMaterial();
    }

    /// <summary>
    /// The AssignMaterial method performs several checks to see what type of game object the player is colliding with
    /// using the layers, and apply the appropriate material
    /// </summary>
    void AssignMaterial()
    {
        bool isUnderwaterAndTouchingWater = waterPlane != null 
            && transform.position.y < waterPlane.position.y 
            && waterContactCount > 0;

        if (isTouchingEnemyLand && enemyLandMaterial != null)
        {
            playerCollider.material = enemyLandMaterial;
        }

        else if ((waterContactCount > 0 || isUnderwaterAndTouchingWater) && waterMaterial != null)
        {
            playerCollider.material = waterMaterial;
        }

        else if (CheckIfTouchingGround() && defaultMaterial != null)
        {
            playerCollider.material = defaultMaterial;
        }
    }

    /// <summary>
    /// The CheckIfTouchingGround method checks to see if the player is collidign with an object that has the "Ground"
    /// layer, ensuring that they are grounded.
    /// </summary>
    /// <returns></returns>
    bool CheckIfTouchingGround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerCollider.radius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true; 
            }
        }
        return false;
    }
}
