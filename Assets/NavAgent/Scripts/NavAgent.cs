using UnityEngine;

public class NavAgent : AIAgent
{
    [SerializeField] NavPath navPath;
    [SerializeField] Movement movement;
    [SerializeField, Range(0, 10)] float rotateSpeed = 1;
    public NavNode TargetNode { get;  set; } = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TargetNode = NavNode.GetNearestNavNode(transform.position);
        TargetNode = (navPath != null) ? 
            navPath.GeneratePath(transform.position, transform.position) : 
            NavNode.GetNearestNavNode(transform.position); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetNode != null)
        {
            Vector3 direction = TargetNode.transform.position - transform.position;
            Vector3 force = direction.normalized * movement.maxForce;
            movement.ApplyForce(force);
        }
        if (movement.Velocity.sqrMagnitude > 0) 
        {
           var targetRotation = Quaternion.LookRotation(movement.Velocity);
           transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        }
    }

    public void OnEnterNavNode(NavNode navNode)
    {
        if (navNode == TargetNode)
        {
            TargetNode = navPath.GetNextNavNode(navNode);
            if (TargetNode == null)
            {
                //reached end of path, generate a new one
                TargetNode = navPath.GeneratePath(navNode, NavNode.GetRandomNavNode());
            }

            else
            {
                TargetNode = navNode.Neighbors[Random.Range(0, navNode.Neighbors.Count)];
            } 
        }
    }
}
