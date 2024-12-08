using UnityEngine;
using UnityEngine.AI;

public class ChasingState : NPCBehavior.State
{
    private enum SubState { ChasingPlayer, SearchingForPlayer }
    private SubState currentSubState = SubState.ChasingPlayer;
    public ChasingState(NPCBehavior npc) : base(npc) { }

    /// <summary>
    /// The OnStateEnter method sets the speed of the npc and sets the current substate to the chase state
    /// </summary>
    public override void OnStateEnter()
    {
        npc.agent.speed = npc.chaseSpeed; 
        currentSubState = SubState.ChasingPlayer;  
    }

    /// <summary>
    /// The update method switches the substate to Chase 
    /// If the player is in range, continue chasing, if not, switch to searching state
    /// </summary>
    public override void UpdateState()
    {
        switch (currentSubState)
        {
            case SubState.ChasingPlayer:
                if (npc.PlayerInSight()) 
                {
                    npc.agent.SetDestination(npc.player.position);
                }
                else 
                {
                    currentSubState = SubState.SearchingForPlayer;
                    npc.SwitchState(npc.searchingState);  
                }
                break;

            case SubState.SearchingForPlayer:
                break;
        }
    }
}