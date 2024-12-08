using UnityEngine;

public class MoveCollider : MonoBehaviour
{
    private BoxCollider boxCollider;
    private bool isColliderEnabled = true;

    [SerializeField] private float disableDelay = 5f; 
    [SerializeField] private float reEnableDelay = 5f; 

    /// <summary>
    /// The start method gets the box collider of the rock and starts the routine of moving, enabling and disabling the collider
    /// </summary>
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        StartCoroutine(DisableAndReEnableCollider());   
    }

    /// <summary>
    /// The update method moves the center of the collider dynamically if it's enabled
    /// </summary>
    void Update()
    {
        if (isColliderEnabled && boxCollider != null)
        {
            boxCollider.center = new Vector3(Mathf.Sin(Time.time) * 2, boxCollider.center.y, boxCollider.center.z);
        }
    }

    /// <summary>
    /// The DisableAndReEnableCollider method disables and re-enables the box collider after a delay
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator DisableAndReEnableCollider()
    {
        while (true)
        {
            yield return new WaitForSeconds(disableDelay);

            if (boxCollider != null)
            {
                boxCollider.enabled = false;
                isColliderEnabled = false;
            }

            yield return new WaitForSeconds(reEnableDelay);

            if (boxCollider != null)
            {
                boxCollider.enabled = true;
                isColliderEnabled = true;
            }
        }
    }
}
