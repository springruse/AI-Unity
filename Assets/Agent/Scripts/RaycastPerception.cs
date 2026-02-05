using System.Collections.Generic;
using UnityEngine;


public class RaycastPerception : Perception
{
    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();

        Vector3[] directions = Utilities.GetDirectionsInCircle(numRays, MaxHalfAngle);

        foreach (var direction in directions)
        {
            GameObject go = GetGameObjectInDirection(transform.rotation * direction);
            if (go != null)
            {
                result.Add(go);
            }
        }

        return result.ToArray();
    }

    public override GameObject GetGameObjectInDirection(Vector3 direction)
    {
        // create ray from transform postion in the direction
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
        {
            // do not include ourselves
            if (raycastHit.collider.gameObject == gameObject) return null;
            // check for matching tag
            if (tagName == "" || raycastHit.collider.CompareTag(tagName))
            {
                // add game object to results
                if (debugMode) Debug.DrawRay(ray.origin, ray.direction *
                raycastHit.distance, Color.red);
                return raycastHit.collider.gameObject;
            }
        }
        else
        {
            if (debugMode) Debug.DrawRay(ray.origin, ray.direction * maxDistance,
            Color.green);
        }
        return null;
    }


    public override bool GetOpenDirection(ref Vector3 openDirection)
    {
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRays, MaxHalfAngle);

        foreach (var direction in directions)
        {
            GameObject go = GetGameObjectInDirection(transform.rotation * direction);
            if (go == null)
            {
                openDirection = transform.rotation * direction;
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRays, MaxHalfAngle);
        foreach (var direction in directions)
        {
            Gizmos.color = debugRayColor;
            Gizmos.DrawRay(transform.position, transform.rotation * direction * maxDistance);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
