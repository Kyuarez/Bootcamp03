using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float openSpeed = 2.0f;

    private BoxCollider doorCol;
    private float targetYaw = 0f;
    private float currentYaw = 0f;
    private bool isOpen = false;
    private bool isOnTarget = false;
    

    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            if(value == false)
            {
                doorCol.enabled = true;
                targetYaw = 0f;
            }
            else
            {
                doorCol.enabled = false;
                if (transform.IsTargetInFront(Operator.Instance.PlayerManager.transform))
                {
                    targetYaw = -135f;  
                }
                else if (transform.IsTargetInBack(Operator.Instance.PlayerManager.transform))
                {
                    targetYaw = 135f;
                }
                else
                {
                    targetYaw = -135f;
                }
            }
            isOpen = value;
        }
    }

    private void Awake()
    {
        doorCol = door.GetComponent<BoxCollider>();

        if(doorCol == null)
        {
           doorCol = door.AddComponent<BoxCollider>();       
        }
    }

    private void Update()
    {
        if(currentYaw != targetYaw)
        {
            currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * openSpeed);
            door.transform.localRotation = Quaternion.Euler(-90f, 0f, currentYaw);
        }

        if(isOnTarget == true)
        {
            if (Input.GetKeyDown(KeyCode.H) == true)
            {
                IsOpen = !IsOpen;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            isOnTarget = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            isOnTarget = false;
        }
    }
}
