using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float defaultSpeed = 5.0f;
    private float sprintSpeed = 10.0f;
    private float currentSpeed;
    private float jumpForce = 10f; 
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;

    public GameObject standingModel;
    public GameObject crouchingModel;

    private CapsuleCollider playerCollider;

    /// <summary>
    /// The start method gets the rigidbody and collider of the player, sets the current speed to the default speed and sets
    /// the standing model as active
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        currentSpeed = defaultSpeed; 

        standingModel.SetActive(true);
        crouchingModel.SetActive(false);

    }

    /// <summary>
    /// The OnCollisionEnter method checks if the player is grounded when initially colliding
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        CheckGrounded(collision);
    }

    /// <summary>
    /// The OnCollisionStay method continuously checks if the player is grounded during collision
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        CheckGrounded(collision);
    }

    /// <summary>
    /// The OnCollisionExit method resets 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionExit(Collision collision)
    {
        // Only reset grounded state when leaving ground or enemy land
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            collision.gameObject.layer == LayerMask.NameToLayer("EnemyLand"))
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// The CheckGrounded method checks to see if the player is colliding with objects with the layers "Ground" and "EnemyLand"
    /// ensuring that the player can jumo on these layers
    /// </summary>
    /// <param name="collision"></param>
    void CheckGrounded(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < 45f) 
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    isGrounded = true;
                }
                else if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyLand"))
                {
                    isGrounded = true;
                }
                return;
            }
        }
        isGrounded = false;
    }

    /// <summary>
    /// The update method contains the logic for the movement mechanics, allowing the player to move
    /// forward, backward, left, right, jump, and crouch
    /// </summary>
    void Update()
    {
        // Movement logic
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f; 
        right.y = 0f;   

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = defaultSpeed;
        }

        // Jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            isGrounded = false;
        }

        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            SwitchToCrouching();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            SwitchToStanding();
        }
    }

    /// <summary>
    /// The Jump method adds upward force to the player to simulate a jump
    /// </summary>
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// The SwitchToCrouching method disables the standing method and activates the crouching model
    /// </summary>
    void SwitchToCrouching()
    {
        standingModel.SetActive(false);
        crouchingModel.SetActive(true);
    }

    /// <summary>
    /// The SwitchToStanding method disables the crouching method and activates the standing model
    /// </summary>
    void SwitchToStanding()
    {
        standingModel.SetActive(true);
        crouchingModel.SetActive(false);
    }
}
