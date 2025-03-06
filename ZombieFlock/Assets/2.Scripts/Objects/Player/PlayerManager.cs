using System.Collections.Generic;
using UnityEngine;
using TKCamera;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 100.0f; //���콺 ����
    
    public Transform camTransform;
    public CharacterController characterController;
    public Transform playerHead; //�÷��̾� �Ӹ� ��ġ(1��Ī cam)
    public float thirdPersonDistance = 3.0f; //�÷��̾� - cam �Ÿ�
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f); 
    public Transform playerLookObj; //�÷��̾� �þ� ��ġ (��� �����)

    //Sight
    public float zoomDistance = 1.0f; //3��Ī
    public float zoomSpeed = 5.0f;
    public float defaultFov = 60.0f;
    public float zoomFov = 30.0f; //Ȯ�� �� ī�޶� �þ߰� (1��Ī)

    private float currentDistance; //���� ī�޶���� �Ÿ� (3��Ī)
    private float targetDistance; //��ǥ ī�޶� �Ÿ�
    private float targetFov;
    private bool isZoomed = false;
    private Coroutine zoomCoroutine; //�ڷ�ƾ ����Ͽ� Ȯ�� ��� ó��
    private Camera mainCam;

    private float pitch = 0.0f; //��-�Ʒ� (�λ�)
    private float yaw = 0.0f; //�¿� ȸ����
    private bool isFirstPerson = false; //1��Ī ��� ����
    private bool isRotaterAroundPlayer = true; //ī�޶� �÷��̾� ������ ȸ���ϴ��� ����

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

        //1��Ī, 3��Ī ����
        if (Input.GetKeyDown(KeyCode.V) == true)
        {
            isFirstPerson = !isFirstPerson;
        }
        //�÷��̾� �ֺ� �ڵ� ȸ�� ����
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
        direction.y = 0; //1��Ī �� �Ʒ� �����̸� ���� �� ��.(�� ����.)
        characterController.Move(direction * moveSpeed * Time.deltaTime);

        //cam ��ġ, 1��Ī ó��
        camTransform.position = playerHead.transform.position;
        camTransform.rotation = Quaternion.Euler(pitch, yaw, 0); //�þ� ������
        transform.rotation = Quaternion.Euler(0f, camTransform.eulerAngles.y, 0); //�� �������� �¿� ȸ���� ����
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
        if (isRotaterAroundPlayer == true) //�÷��̾ ���� shoulderView
        {
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            camTransform.position = transform.position + thirdPersonOffset + rotation * direction;
            camTransform.LookAt(transform.position + new Vector3(0, thirdPersonOffset.y, 0));
        }
        else //�÷��̾ ���� �������� ī�޶� ���� shoulderView
        {
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            Vector3 direction = new Vector3(0, 0, -currentDistance);
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            camTransform.position = playerLookObj.position + thirdPersonOffset + rotation * direction;
            camTransform.LookAt(playerLookObj.position + new Vector3(0, thirdPersonOffset.y, 0));
        }
    }
}
