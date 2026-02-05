using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    [SerializeField] string info;
    [SerializeField] protected string tagName;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField, Range (0,10)] protected float maxDistance = 3;
    [SerializeField, Range (0,10)] protected int numRays = 4;
    [SerializeField, Range(0,180)] protected float MaxHalfAngle = 180;

    [Header("Debug")]
    [SerializeField] protected bool debugMode = false;
    [SerializeField] protected Color debugRayColor = Color.red;

    public abstract GameObject[] GetGameObjects();

    public virtual GameObject GetGameObjectInDirection(Vector3 direction) { return null; }
    public virtual bool GetOpenDirection(ref Vector3 direction) { return false; }

}
