using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    public string tagName;
    public float maxDistance;
    public float maxViewAngle;

    public abstract GameObject[] getGameObjects();
}
