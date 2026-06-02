using UnityEngine;

public class WaypointGizmo : MonoBehaviour
{
    public float sphereRadius = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
}
