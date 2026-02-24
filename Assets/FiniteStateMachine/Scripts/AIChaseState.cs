using UnityEngine;

public class AIChaseState : AIState
{
    public AIChaseState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        agent.movement.maxSpeed *= 2.0f;
    }

    public override void OnExit()
    {
        agent.movement.maxSpeed /= 2.0f;
    }

    public override void OnUpdate()
    {
        if (agent.enemy != null)
        {
            agent.movement.Destination = agent.enemy.transform.position;

            //set state to attack when in distance
            if (agent.distanceToEnemy <= 1.5f)
            {
                
                //agent.StateMachine.SetState<AIAttackState>();
                agent.PushDownStateMachine.PushState<AIAttackState>();
            }
        }
        else
        {
            //set state to idle when enemy is no longer seen
            //agent.StateMachine.SetState<AIIdleState>();
            agent.PushDownStateMachine.PopState();
        }
    }
}
