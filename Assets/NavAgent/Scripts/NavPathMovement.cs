using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavPath))]
public class NavPathMovement : KinematicMovement
{
    NavPath navPath = null;
    public NavNode targetNavNode { get; set; } = null;

    private void Awake()
    {
        navPath = GetComponent<NavPath>();
    }

    public override Vector3 Destination
    {
        get => targetNavNode.transform.position;
        set => targetNavNode = navPath.GeneratePath(transform.position, value);
    }

    public void OnEnterNavNode(NavNode navNode)
    {
        if (navNode == targetNavNode)
        {
            // get next nav node in path, returns null if no next
            targetNavNode = navPath.GetNextNavNode(navNode);
        }
    }
}
