using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 50f;

    /// <summary>
    /// The update method roates the coin
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// The OnTriggerEnter method disables the coin upon upon trigger collision with the player
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You got a Coin!");
            gameObject.SetActive(false);
        }
    }
}
