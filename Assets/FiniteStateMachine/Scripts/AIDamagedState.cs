using UnityEngine;

public class AIDamagedState : AIState
{
    public AIDamagedState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        agent.timer = 1.5f;
        agent.animator.SetTrigger("Damage");
        agent.movement.Destination = agent.transform.position; // stop moving
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (agent.timer <= 0.0f)
        {
            //set state to idle
            //agent.PushDownStateMachine.SetState<AIIdleState>();
            agent.PushDownStateMachine.PopState();
            agent.PushDownStateMachine.PushState<AIIdleState>();
        }

        
    }
}
