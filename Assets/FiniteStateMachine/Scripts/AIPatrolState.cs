using UnityEngine;

public class AIPatrolState : AIState
{
    public AIPatrolState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        agent.Destination = NavNode.GetRandomNavNode().transform.position;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        //check if reached destination
        if (agent.distanceToDestination <= 1.5f)
        {
            //set state to idle
            //agent.StateMachine.SetState<AIIdleState>();
            agent.PushDownStateMachine.PopState();
            return;
        }

        //check if enemy is in sight
        if (agent.enemy != null)
        {
            //set state to chase
            //agent.StateMachine.SetState<AIChaseState>();
            agent.PushDownStateMachine.PushState<AIChaseState>();
        }
    }
}
