using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TKCamera;

public class PlayerManager : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 18.0f;
    public float mouseSensitivity = 100.0f; //���콺 ����
    private float moveSpeed;
    
    public Transform camTransform;
    public Transform playerHead; //�÷��̾� �Ӹ� ��ġ(1��Ī cam)
    public Transform WeaponTransform;
    public float thirdPersonDistance = 3.0f; //�÷��̾� - cam �Ÿ�
    public float immersionDistance = 1.0f; //3��Ī ������ �Ÿ�
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f); 
    public Transform playerLookObj; //�÷��̾� �þ� ��ġ (��� �����)

    //Sight
    public float zoomDistance = 1.0f; //3��Ī
    public float zoomImmersionDistance = 0.75f; 
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
    private bool isImmersion = false; //���� ��� ����
    private bool isRotaterAroundPlayer = true; //ī�޶� �÷��̾� ������ ȸ���ϴ��� ����

    //Gravity
    private CharacterController characterController;
    public float gravity = -9.81f; 
    public float jump = 2.0f;
    private Vector3 velocity;
    private bool isGround;

    //Anim
    private Animator anim;
    private float horizontal;
    private float vertical;
    private bool isRunning = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = thirdPersonDistance;
        targetDistance = thirdPersonDistance;
        targetFov = defaultFov;
        mainCam = camTransform.GetComponent<Camera>();
        mainCam.fieldOfView = defaultFov;

        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        moveSpeed = walkSpeed;
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


        UpdateSight();
        Zoom();

        SetMoveState();
        SetAnimation();
    }

    private void UpdateSight()
    {
        //1��Ī, 3��Ī ����
        if (Input.GetKeyDown(KeyCode.V) == true)
        {
            isFirstPerson = !isFirstPerson;
        }
        //�÷��̾� �ֺ� �ڵ� ȸ�� ����
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            isRotaterAroundPlayer = !isRotaterAroundPlayer;
        }
        //������ ������� ����
        if(Input.GetKeyDown(KeyCode.M) == true)
        {
            isImmersion = !isImmersion;
            targetDistance = (isImmersion == false) ? thirdPersonDistance : immersionDistance;
            currentDistance = targetDistance;
        }

        if (isFirstPerson == true)
        {
            FirstPersonMovement();
        }
        else
        {
            ThirdPersonMovement();
        }
    }

    private void Zoom()
    {
        //Zoom part
        if (Input.GetMouseButtonDown(1) == true)
        {
            //Coroutine ���� ���ؼ�, ����ȭ(�޼ҵ� �� ���� ������ �̷��� ����)
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }

            if (isFirstPerson == true)
            {
                SetTargetFOV(zoomFov);
                zoomCoroutine = StartCoroutine(ZoomFieldOfViewCo(targetFov));
            }
            else
            {
                float zoomValue = (isImmersion == false) ? zoomDistance : zoomImmersionDistance;
                SetTargetDistance(zoomValue);
                zoomCoroutine = StartCoroutine(ZoomCameraCo(targetDistance));
            }
        }

        if (Input.GetMouseButtonUp(1) == true)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }

            if (isFirstPerson == true)
            {
                SetTargetFOV(defaultFov);
                zoomCoroutine = StartCoroutine(ZoomFieldOfViewCo(targetFov));
            }
            else
            {
                float zoomValue = (isImmersion == false) ? thirdPersonDistance : immersionDistance;
                SetTargetDistance(zoomValue);
                zoomCoroutine = StartCoroutine(ZoomCameraCo(targetDistance));
            }
        }
    }

    private void SetAnimation()
    {
        //Anim
        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
        anim.SetBool("IsRunning", isRunning);
    }

    private void SetMoveState()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) == true)
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) == true)
        {
            isRunning = false;
        }

        moveSpeed = (isRunning == true) ? runSpeed : walkSpeed;
    }

    private void FirstPersonMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

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
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(direction * moveSpeed * Time.deltaTime);
        
        UpdateCameraPosition();
    }

    public void SetTargetDistance(float distance)
    {
        targetDistance = distance;
    }
    public void SetTargetFOV(float fov)
    {
        targetFov = fov;
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

    #region Coroutine
    /// <summary>
    /// 3��Ī Zoom : distance Lerp
    /// </summary>
    IEnumerator ZoomCameraCo(float targetDistance)
    {
        while (Mathf.Abs(currentDistance - targetDistance) > 0.01f)
        {
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        currentDistance = targetDistance;
    }

    /// <summary>
    /// 1��Ī Zoom : fov ����
    /// </summary>
    IEnumerator ZoomFieldOfViewCo(float targetDistance)
    {
        while(Mathf.Abs(mainCam.fieldOfView - targetFov) > 0.01f)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        mainCam.fieldOfView = targetFov;
    }
    #endregion
}
