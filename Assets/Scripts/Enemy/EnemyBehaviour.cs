using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    [Header("Patrolling Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public int currentWaypointIndex = 0;

    [Header("Chase Settings")]
    public float sightRange = 15f;
    public Transform player;
    public float chaseSpeed = 7f;
    public LayerMask obstructionMask;

    [Header("Search Settings")]
    public float searchDuration = 5f;
    public float searchRotationSpeed = 100f;

    public NavMeshAgent agent;
    public State currentState;

    public PatrollingState patrollingState;
    public ChasingState chasingState;
    public SearchingState searchingState;

    public float searchTimer;
    public Vector3 lastWaypointPosition;

    private float raycastInterval = 0.5f;
    private bool canSeePlayer = false; 
    private float raycastTimer = 0f;

    private Rigidbody rb;
    private bool isKnockedBack = false;
    public float knockbackResetDelay = 0.5f;

    /// <summary>
    /// The start method gets the NavMeshAgent of the NPC and sets
    /// the initial states
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("Waypoints are not assigned!");
            enabled = false;
            return;
        }

        patrollingState = new PatrollingState(this);
        chasingState = new ChasingState(this);
        searchingState = new SearchingState(this);

        currentState = patrollingState;
        agent.speed = patrolSpeed;
        patrollingState.OnStateEnter();

        StartCoroutine(CheckPlayerSight());
    }

    /// <summary>
    /// The update method updates the state based on the current state, checks if it is time to perform
    /// the raycast check, then resets the timer for the next raycast check
    /// </summary>
    void Update()
    {
        raycastTimer -= Time.deltaTime;

        if (raycastTimer <= 0)
        {
            raycastTimer = raycastInterval; 
            canSeePlayer = PlayerInSight();
        }

        if (!isKnockedBack)
        {
            currentState?.UpdateState();
        }
    }

    /// <summary>
    /// The SwitchState method switches to a new state and calls OnStateEnter on the new state
    /// </summary>
    /// <param name="newState"></param>
    public void SwitchState(State newState)
    {
        currentState = newState;
        newState.OnStateEnter();
    }

    /// <summary>
    /// Base state class
    /// </summary>
    public abstract class State
    {
        protected NPCBehavior npc;

        public State(NPCBehavior npc)
        {
            this.npc = npc;
        }

        public virtual void OnStateEnter() { }
        public abstract void UpdateState();
    }

    /// <summary>
    /// The PlayerInSight method checks if the player is in sight range
    /// </summary>
    /// <returns></returns>
    public bool PlayerInSight()
    {
        if (player == null)
        {
            return false;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// The CheckPlayerSight method employs the coroutine to check vision of the player at a limited interval
    /// </summary>
    private System.Collections.IEnumerator CheckPlayerSight()
    {
        while (true)
        {
            yield return new WaitForSeconds(raycastInterval); 
            canSeePlayer = PlayerInSight(); 
        }
    }

    /// <summary>
    /// Applies knockback to the NPC and temporarily disables its NavMeshAgent.
    /// </summary>
    /// <param name="direction">Direction of the knockback force.</param>
    /// <param name="force">Magnitude of the knockback force.</param>
    public void ApplyKnockback(Vector3 direction, float force)
    {
        direction = direction.normalized;

        if (agent.enabled)
        {
            agent.enabled = false;
        }

        isKnockedBack = true;
        rb.isKinematic = false;
        rb.AddForce(direction * force, ForceMode.Impulse);

        Invoke(nameof(EndKnockback), knockbackResetDelay);
    }

    /// <summary>
    /// The EndKnockback method ends the knockback effect and re-enables the NavMeshAgent
    /// </summary>
    private void EndKnockback()
    {
        isKnockedBack = false;

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        if (!agent.enabled)
        {
            agent.enabled = true;
        }
    }
}
