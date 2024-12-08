using UnityEngine;
using UnityEngine.AI;

public class PatrollingState : NPCBehavior.State
{
    private enum SubState { MovingToWaypoint, Waiting, Detouring }
    private SubState currentSubState = SubState.MovingToWaypoint;

    private float waitTimer = 2f; 
    private float waitTime = 2f; 

    public PatrollingState(NPCBehavior npc) : base(npc) { }

    /// <summary>
    /// The OnStateEnter method sets the NPC's speed, currentsubstate, and current waypoint
    /// </summary>
    public override void OnStateEnter()
    {
        npc.agent.speed = npc.patrolSpeed;
        currentSubState = SubState.MovingToWaypoint;
        npc.currentWaypointIndex = 0;
    }

    /// <summary>
    /// The UpdateState checks if the player is in range of the enemy before tranisitioning to the chasing state
    /// If not, move from waypoint to waypoint
    /// When reaching a waypoint, the enemy has a 20% chance to pick a random waypoint, and has a 40% chance to
    /// transition into the searching state
    /// </summary>
    public override void UpdateState()
    {
        if (npc.waypoints.Length == 0) return;

        if (npc.PlayerInSight())
        {
            npc.SwitchState(npc.chasingState);
            return;
        }

        switch (currentSubState)
        {
            case SubState.MovingToWaypoint:
                npc.agent.SetDestination(npc.waypoints[npc.currentWaypointIndex].position);

                if (!npc.agent.pathPending && npc.agent.remainingDistance < 0.5f)
                {
                    currentSubState = SubState.Waiting;
                    waitTimer = waitTime;
                }
                break;

            case SubState.Waiting:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    float randomChance = Random.Range(0f, 1f);

                    if (randomChance < 0.2f)
                    {
                        Debug.Log("Taking a random detour.");
                        npc.agent.SetDestination(npc.waypoints[Random.Range(0, npc.waypoints.Length)].position);
                        currentSubState = SubState.Detouring;
                        return;
                    }

                    else if (randomChance < 0.4f)
                    {
                        Debug.Log("Transitioning to Searching State.");
                        npc.SwitchState(npc.searchingState);
                        return;
                    }

                    npc.currentWaypointIndex = (npc.currentWaypointIndex + 1) % npc.waypoints.Length;
                    currentSubState = SubState.MovingToWaypoint;
                    Debug.Log("Waiting finished, moving to next waypoint.");
                }
                break;

            case SubState.Detouring:
                if (!npc.agent.pathPending && npc.agent.remainingDistance < 0.5f)
                {
                    currentSubState = SubState.Waiting;
                    Debug.Log("Detour completed, returning to waiting state.");
                }
                break;
        }
    }
}