using System.Collections.Generic;
using UnityEngine; 

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 100.0f; //마우스 감도
    
    public Transform camTransform;
    public CharacterController characterController;
    public Transform playerHead; //플레이어 머리 위치(1인칭 cam)
    public float thirdPersonDistance = 3.0f; //플레이어 - cam 거리
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f); 
    public Transform playerLookObj; //플레이어 시야 위치 (배그 숄더숏)

    public float zoomDistance = 1.0f; //3인칭
    public float zoomSpeed = 5.0f;
    public float defaultFov = 60.0f;
    public float zoomFov = 30.0f; //확대 시 카메라 시야각 (1인칭)


    private void Start()
    {

    }

    private void Update()
    {
        
    }
}
