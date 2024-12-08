using UnityEngine;

public class Star : MonoBehaviour
{
    public float rotationSpeed = 50f;

    /// <summary>
    /// The update method rotates the star anticlockwise along the Y-axis
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
