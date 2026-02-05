using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NavPath : MonoBehaviour
{
    List<NavNode> path = new List<NavNode>();

    public NavNode GeneratePath(Vector3 startPosition, Vector3 endPosition)
    {
        NavNode startNode = NavNode.GetNearestNavNode(startPosition);
        NavNode endNode = NavNode.GetNearestNavNode(endPosition);

        GeneratePath(startNode, endNode);
        //generate path

        return startNode;

    }
    public NavNode GetNextNavNode(NavNode navNode)
    {
        if (path.Count == 0) return null;

        int index = path.IndexOf(navNode);
        return (index == -1 || index + 1 >= path.Count) ? null : path[index + 1];
    }

    public NavNode GeneratePath(NavNode startNode, NavNode endNode)
    {

        path.Clear();
        NavNode.ResetNavNodes();

        //generate path
        //NavDijkstra.Generate(startNode, endNode, ref path);
        NavAstar.Generate(startNode, endNode, ref path);

        return startNode;

    }
    private void OnDrawGizmosSelected()
    {
        if (path.Count == 0) return;

        // Draw intermediate nodes (white)
        for (int i = 1; i < path.Count - 1; i++)
        {
            Gizmos.color = Utilities.white;
            Gizmos.DrawSphere(path[i].transform.position, 1);
        }

        // Draw start node (blue) and end node (red)
        Gizmos.color = Utilities.blue;
        Gizmos.DrawSphere(path[0].transform.position, 1);

        Gizmos.color = Utilities.red;
        Gizmos.DrawSphere(path[^1].transform.position, 1);
    }

}
