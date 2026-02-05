using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class NavNode : MonoBehaviour
{
    [SerializeField] protected List<NavNode> neighbors;
    public List<NavNode> Neighbors { get { return neighbors; } set { neighbors = value; } }
    public float Cost { get; set; } = 0;
    public NavNode PreviousNode { get; set; } = null;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors)
        {
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // check if collider is a nav agent
        if (other.gameObject.TryGetComponent<NavAgent>(out NavAgent navAgent))
        {
            navAgent.OnEnterNavNode(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // check if collider is a nav agent
        if (other.gameObject.TryGetComponent<NavAgent>(out NavAgent navAgent))
        {
            navAgent.OnEnterNavNode(this);
        }
    }

    #region helper_functions

    public static NavNode[] GetAllNavNodes()
    {
        return FindObjectsByType<NavNode>(FindObjectsSortMode.None);
    }

    public static NavNode GetRandomNavNode()
    {
        var navNodes = GetAllNavNodes();
        return (navNodes.Length == 0) ? null : navNodes[Random.Range(0, navNodes.Length)];
    }

    public static NavNode GetNearestNavNode(Vector3 position)
    {
        NavNode nearestNavNode = null;
        float nearestDistance = float.MaxValue;
        var navNodes = GetAllNavNodes();

        foreach (var navNode in navNodes)
        {
            float distance = Vector3.Distance(navNode.transform.position, position);
            if (distance < nearestDistance)
            {
                nearestNavNode = navNode;
                nearestDistance = distance;
            }
        }
        return nearestNavNode;
    }

    public static void ResetNavNodes()
    {
        var navNodes = GetAllNavNodes();

        foreach(var navNode in navNodes)
        {
            navNode.Cost = float.MaxValue;
            navNode.PreviousNode = null;
        }
    }
    public static void CreatePath(NavNode navNode, ref List<NavNode> path)
    {
        // add nodes to our path
        while (navNode != null) 
        {
            path.Add(navNode);
            navNode = navNode.PreviousNode;
        }
        // reverse path (end is first)
        path.Reverse();
    }
    #endregion
}
