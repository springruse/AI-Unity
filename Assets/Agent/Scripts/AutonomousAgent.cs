using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class AutonomousAgent : AIAgent
{
    [SerializeField] bool debug = false;
    [SerializeField] Movement movement;
    [SerializeField] Perception seekPerception;
    [SerializeField] Perception fleePerception;
    [SerializeField] Perception flockPerception;
    [SerializeField] Perception obstaclePerception;

    [Header("Wander")]

    [SerializeField, Range(0, 10)] float wanderRadius = 1;
    [SerializeField, Range(0, 10)] float wanderDistance = 1;
    [SerializeField, Range(0, 10)] float wanderDisplacement = 1;
    float wanderAngle = 0.0f;

    [Header("Flock Weights")]
    [SerializeField, Range(0, 5)] float cohesionWeight = 1;
    [SerializeField, Range(0, 5)] float separationWeight = 1;
    [SerializeField, Range(0, 5)] float alignmentWeight = 1;
    [Header("Flock Radiuses")]
    [SerializeField, Range(0, 5)] float separationRadius = 1;

    [Header("Obstacle Avoidance")]
    [SerializeField, Range(0, 10)] float obstacleAvoidanceWeight = 1;

    void Start()
    {
        wanderAngle = Random.Range(0, 360);
    }

    void Update()
    {
        bool hasTarget = false;

        if (seekPerception != null) {
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }
        if (fleePerception != null)
        {
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        if (flockPerception != null)
        {
            var gameObjects = flockPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                hasTarget = true;
                movement.ApplyForce(Cohesion(gameObjects) * cohesionWeight);
                movement.ApplyForce(Separation(gameObjects, separationRadius) * separationWeight);
                movement.ApplyForce(Alignment(gameObjects) * alignmentWeight);
            }
        }

        if (obstaclePerception != null && obstaclePerception.GetGameObjectInDirection(transform.forward) != null) 
        {
            Vector3 openDirection = Vector3.zero;
            if (obstaclePerception.GetOpenDirection(ref openDirection))
            {
                hasTarget = true;
                movement.ApplyForce(GetSteeringForce(openDirection) * obstacleAvoidanceWeight);
            }
        }

        // if no target then wander
        if (!hasTarget)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }

        //foreach (var go in gameObjects) 
        //{
        //    Debug.DrawLine(transform.position, go.transform.position, Color.magenta);
        //}

        // transform.position = Utilities.Wrap(transform.position, new Vector3(-10,-10,-10), new Vector3(10, 10, 10));

        // Only wrap X and Z, keep Y unchanged
        Vector3 pos = transform.position;
        pos.x = Utilities.Wrap(pos.x, -10, 10);
        pos.z = Utilities.Wrap(pos.z, -10, 10);
        pos.y = 0;
        // Don't wrap Y - agents stay at their current height
        transform.position = pos;

        if (movement.Velocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Velocity, Vector3.up);
        }
        
    }

    public Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);
        return force;
    }

   Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    Vector3 Flee(GameObject go)
    {
        Vector3 direction = transform.position - go.transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    Vector3 Wander()
    {
        wanderAngle += Random.Range(-wanderDisplacement, wanderDisplacement);
        Quaternion rotation = Quaternion.AngleAxis(wanderAngle, Vector3.up);
        Vector3 pointOnCircle = rotation * Vector3.forward * wanderRadius;
        Vector3 circleCenter = movement.Direction.normalized * wanderDistance;
        Vector3 force = GetSteeringForce(circleCenter +pointOnCircle);

        if (debug)
        {
            Debug.DrawLine(transform.position, transform.position + circleCenter, Color.blue);
            Debug.DrawLine(transform.position, transform.position + circleCenter + pointOnCircle, Color.red);
        }

        return force;
    }

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        // accumulate the position vectors of the neighbors
        foreach (var neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }

        // average the positions to get the center of the neighbors
        Vector3 center = positions / neighbors.Length;
        // create direction vector to point towards the center of the neighbors from agent position
        Vector3 direction = center - transform.position;

        // steer towards the center point
        Vector3 force = GetSteeringForce(direction);

    return force;
    }

    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;
        // accumulate the separation vectors of the neighbors
        foreach (var neighbor in neighbors)
        {
            // get direction vector away from neighbor
            Vector3 direction = transform.position - neighbor.transform.position;
            float distance = direction.magnitude;

            // check if within separation radius
            if (distance > 0 && distance < radius)
            {
                // scale separation vector inversely proportional to the direction distance
                // closer the distance the stronger the separation
                separation += direction * (1 / distance);
            }
        }
        // steer towards the separation point
        Vector3 force = (separation.magnitude > 0) ? GetSteeringForce(separation) : Vector3.zero;

        return force;
    }

    private Vector3 Alignment(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;
        // accumulate the velocity vectors of the neighbors
        foreach (var neighbor in neighbors)
        {
            if (neighbor.TryGetComponent(out AutonomousAgent component))
            {
                velocities += component.movement.Velocity;
            }
        }
        // get the average velocity of the neighbors
        Vector3 averageVelocity = velocities / neighbors.Length;
        // steer towards the average velocity
        Vector3 force  = GetSteeringForce(averageVelocity);

        return force;
    }


}
