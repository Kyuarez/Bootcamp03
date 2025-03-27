using UnityEngine;

public class ChapterEndPoint : MonoBehaviour
{
    private SphereCollider col;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") == true)
        {
            QuestEventManager.TriggerEvent(QuestEventType.EnterEndPoint);
            col.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        float radius = col.radius;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
