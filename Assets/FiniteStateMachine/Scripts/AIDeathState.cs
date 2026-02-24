using UnityEngine;
using UnityEngine.AI;

public class AIDeathState : AIState
{
    public AIDeathState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        agent.animator.SetTrigger("Death");
        agent.movement.Destination = agent.transform.position; // stop moving
        agent.gameObject.GetComponent<NavMeshAgent>().enabled = false; // disable navmesh agent
        GameObject.Destroy(agent.gameObject, 5.0f);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {

    }
}
