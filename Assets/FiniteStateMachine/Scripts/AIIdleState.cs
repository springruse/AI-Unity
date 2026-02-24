using System;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIIdleState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        agent.timer = 2.0f;
    }

    public override void OnExit()
    {


    }

    public override void OnUpdate()
    {
        if (agent.timer <= 0.0f)
        {
            // set state to patrol
            agent.PushDownStateMachine.PushState<AIPatrolState>();
            return;
            //agent.StateMachine.SetState<AIPatrolState>();
        }

        if (agent.enemy != null)
        {
            //set state to chase
            agent.PushDownStateMachine.PushState<AIChaseState>();
            //agent.StateMachine.SetState<AIChaseState>();
        }

    }
}
