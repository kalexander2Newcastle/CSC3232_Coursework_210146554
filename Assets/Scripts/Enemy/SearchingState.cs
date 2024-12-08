using UnityEngine;

public class SearchingState : NPCBehavior.State
{
    private enum SubState { RotatingClockwise, RotatingCounterclockwise }
    private SubState currentSubState = SubState.RotatingClockwise;

    private float totalRotation = 0f; 
    private float targetRotation = 90f; 

    public SearchingState(NPCBehavior npc) : base(npc) { }

    /// <summary>
    /// The OnStateEnter method sets the NPC's search duration, total rotation, and current substate
    /// </summary>
    public override void OnStateEnter()
    {
        npc.searchTimer = npc.searchDuration;
        npc.agent.isStopped = true;
        totalRotation = 0f; 
        currentSubState = SubState.RotatingClockwise;
        Debug.Log("NPC has entered Searching State");
    }

    /// <summary>
    /// The UpdateState method rotates the NPC 90 degrees either direction to simulate searching.
    /// When the search ends, the NPC returns to the patrol
    /// </summary>
    public override void UpdateState()
    {
        npc.searchTimer -= Time.deltaTime;

        if (npc.searchTimer > 0)
        {
            switch (currentSubState)
            {
                case SubState.RotatingClockwise:
                    float clockwiseRotation = npc.searchRotationSpeed * Time.deltaTime;
                    npc.transform.Rotate(0, clockwiseRotation, 0);
                    totalRotation += clockwiseRotation;

                    if (totalRotation >= targetRotation)
                    {
                        currentSubState = SubState.RotatingCounterclockwise;
                        totalRotation = 0f;
                    }
                    break;

                case SubState.RotatingCounterclockwise:
                    float counterClockwiseRotation = -npc.searchRotationSpeed * Time.deltaTime;
                    npc.transform.Rotate(0, counterClockwiseRotation, 0);
                    totalRotation += Mathf.Abs(counterClockwiseRotation); 

                    if (totalRotation >= targetRotation)
                    {
                        currentSubState = SubState.RotatingClockwise;
                        totalRotation = 0f; 
                    }
                    break;
            }
        }
        else
        {
            npc.agent.isStopped = false;
            npc.agent.SetDestination(npc.lastWaypointPosition);
            Debug.Log("Search timer expired. Switching back to Patrolling State.");
            npc.SwitchState(npc.patrollingState);
        }
    }
}