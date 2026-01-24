using System.Collections.Generic;
using UnityEngine;

public class RaycastPerception : Perception
{
    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();

        Vector3[] directions = new Vector3[3];
        directions[0] = Quaternion.Euler(0, -25, 0) * Vector3.forward; // Left ray
        directions[1] = Vector3.forward;                                 // Center ray
        directions[2] = Quaternion.Euler(0, 25, 0) * Vector3.forward;   // Right ray

        foreach (var direction in directions)
        {
            Ray ray = new Ray(transform.position, transform.rotation * direction);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
            {
                if (raycastHit.collider.gameObject == gameObject) continue;

                if (tagName == "" || raycastHit.collider.CompareTag(tagName))
                {
                    result.Add(raycastHit.collider.gameObject);
                    Debug.DrawRay(ray.origin, ray.direction * raycastHit.distance, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            }
        }
        return result.ToArray();
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
