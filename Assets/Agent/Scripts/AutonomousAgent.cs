using UnityEngine;

public class AutonomousAgent : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] Perception perception;
    void Start()
    {
        
    }

    void Update()
    {
        movement.ApplyForce(Vector3.forward);
        transform.position = Utilities.Wrap(new Vector3(15,15,15), new Vector3(0, 0, 0), new Vector3(15, 15, 15));
    }
}
