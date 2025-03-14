using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    private float radius = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position ,radius);
    }
}
