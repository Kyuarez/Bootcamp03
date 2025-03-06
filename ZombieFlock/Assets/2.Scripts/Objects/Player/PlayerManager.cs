using System.Collections.Generic;
using UnityEngine;
using TKCamera;

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

    //Sight
    public float zoomDistance = 1.0f; //3인칭
    public float zoomSpeed = 5.0f;
    public float defaultFov = 60.0f;
    public float zoomFov = 30.0f; //확대 시 카메라 시야각 (1인칭)

    private float currentDistance; //현재 카메라와의 거리 (3인칭)
    private float targetDistance; //목표 카메라 거리
    private float targetFov;
    private bool isZoomed = false;
    private Coroutine zoomCoroutine; //코루틴 사용하여 확대 축소 처리
    private Camera mainCam;

    private float pitch = 0.0f; //위-아래 (인사)
    private float yaw = 0.0f; //좌우 회전값
    private bool isFirstPerson = false; //1인칭 모드 여부
    private bool isRotaterAroundPlayer = true; //카메라가 플레이어 주위를 회전하는지 여부

    //Gravity
    public float gravity = -9.81f; 
    public float jump = 2.0f;
    private Vector3 velocity;
    private bool isGround;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = thirdPersonDistance;
        targetDistance = thirdPersonDistance;
        targetFov = defaultFov;
        mainCam = camTransform.GetComponent<Camera>();
        mainCam.fieldOfView = defaultFov;
    }
    
    private void Update()
    {
        #region Test
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            CameraShake shake = mainCam.GetComponent<CameraShake>();
            if (shake != null) 
            {
                shake.ExplosiveCameraShake();
            }
        }

        #endregion

        //Mouse Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        pitch = Mathf.Clamp(pitch, -45, 45);

        isGround = characterController.isGrounded;
        if (isGround == true && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        //1인칭, 3인칭 결정
        if (Input.GetKeyDown(KeyCode.V) == true)
        {
            isFirstPerson = !isFirstPerson;
        }
        //플레이어 주변 자동 회전 여부
        if(Input.GetKeyDown(KeyCode.F) == true)
        {
            isRotaterAroundPlayer = !isRotaterAroundPlayer;
        }

        if(isFirstPerson == true)
        {
            FirstPersonMovement();
        }
        else
        {
            ThirdPersonMovement();
        }

    }

    private void FirstPersonMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //@tk : camTransform = playerHead
        Vector3 direction = camTransform.right * horizontal + camTransform.forward * vertical;
        direction.y = 0; //1인칭 위 아래 움직이면 절대 안 됨.(눈 아파.)
        characterController.Move(direction * moveSpeed * Time.deltaTime);

        //cam 위치, 1인칭 처리
        camTransform.position = playerHead.transform.position;
        camTransform.rotation = Quaternion.Euler(pitch, yaw, 0); //시야 움직임
        transform.rotation = Quaternion.Euler(0f, camTransform.eulerAngles.y, 0); //몸 움직임은 좌우 회전만 강제
    }

    private void ThirdPersonMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(direction * moveSpeed * Time.deltaTime);
        
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        //ThirdPerson : Shoulder View
        if (isRotaterAroundPlayer == true) //플레이어를 보게 shoulderView
        {
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            camTransform.position = transform.position + thirdPersonOffset + rotation * direction;
            camTransform.LookAt(transform.position + new Vector3(0, thirdPersonOffset.y, 0));
        }
        else //플레이어가 보는 시점으로 카메라 보게 shoulderView
        {
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            camTransform.position = playerLookObj.position + thirdPersonOffset + rotation * direction;
            camTransform.LookAt(playerLookObj.position + new Vector3(0, thirdPersonOffset.y, 0));
        }
    }
}
