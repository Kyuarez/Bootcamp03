using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TKCamera;
using UnityEngine.Animations.Rigging;
using System.Net.Sockets;

public class PlayerManager : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 12.0f;
    public float mouseSensitivity = 100.0f; //���콺 ����
    private float moveSpeed;
    public LayerMask targetMask;

    public Transform camTransform;
    public Transform playerHead; //�÷��̾� �Ӹ� ��ġ(1��Ī cam)
    public Transform WeaponTransform;
    public float thirdPersonDistance = 3.0f; //�÷��̾� - cam �Ÿ�
    public float immersionDistance = 1.0f; //3��Ī ������ �Ÿ�
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f);
    public Transform playerLookObj; //�÷��̾� �þ� ��ġ (��� �����)
    public Transform playerImmersionLookObj; //�÷��̾� �þ� ��ġ : �ٸ� �ڸ���

    //Sight
    public float zoomDistance = 1.0f; //3��Ī
    public float zoomImmersionDistance = 0.5f;
    public float zoomSpeed = 5.0f;
    public float defaultFov = 60.0f;
    public float zoomFov = 30.0f; //Ȯ�� �� ī�޶� �þ߰� (1��Ī)

    private float currentDistance; //���� ī�޶���� �Ÿ� (3��Ī)
    private float targetDistance; //��ǥ ī�޶� �Ÿ�
    private float targetFov;
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
    private bool isAim = false;
    private bool isPickup = false;

    //Rig
    public Transform aimTarget;
    public MultiAimConstraint multiAimConstraint;

    //ItemPickup
    private Vector3 pickupBoxSize = new Vector3(1.0f, 1.0f, 1.0f);
    private float castDistance = 5.0f;
    public LayerMask pickupMask;
    public Transform itemGetPos;

    private float rifleFireDelay = 0.5f;
    private bool isShot = false;
    private BucketManager bucket;

    //@tk : temp
    //sound
    public AudioClip audioClipFire;
    public AudioClip audioClipWeaponChange;
    public AudioSource audioSource;
    //Gun : ���߿� �� Ŭ�������� �������� (bucket)
    private float gunMaxRange = 1000.0f;
    private int bulletDamage = 10;

    public bool IsFirstPerson {  get { return isFirstPerson; } }
    public bool IsImersion {  get { return isImmersion; } }
    public bool IsAim
    {
        get { return isAim; }
        private set
        {
            if (value == true)
            {
                bucket.CurrentWeapon.transform.localPosition = Define.RifleAim_Pos;
                bucket.CurrentWeapon.transform.localRotation = Quaternion.Euler(Define.RifleAim_Rotate);
                multiAimConstraint.data.offset = new Vector3(-30f, 0f, 0f);
            }
            else
            {
                bucket.CurrentWeapon.transform.localPosition = Define.Rifle_Pos;
                bucket.CurrentWeapon.transform.localRotation = Quaternion.Euler(Define.Rifle_Rotate);
                multiAimConstraint.data.offset = new Vector3(0f, 0f, 0f);
            }
            isAim = value;
        }
    }

    public Gun CurrentWeapon
    {
        get
        {
            return bucket.CurrentWeapon;
        }
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        currentDistance = thirdPersonDistance;
        targetDistance = thirdPersonDistance;
        targetFov = defaultFov;
        camTransform = Camera.main.transform;
        mainCam = camTransform.GetComponent<Camera>();
        mainCam.fieldOfView = defaultFov;

        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        bucket = GetComponent<BucketManager>();

        moveSpeed = walkSpeed;
        bucket.InitBucket();
    }

    private void Update()
    {
        UpdateMouseSet();
        CheckIsGrounded();

        EquippedWeapon();

        UpdateSight();
        Zoom();

        SetMoveState();
        SetAnimation();

        OnShot();
        PickupItem();
        PostPickupItem();
    }

    private GameObject adjacentItem;
    private void PickupItem()
    {
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            if (isPickup == true)
            {
                return;
            }

            //TODO : ������ üŷ�ؼ� ���� ó��
            Vector3 origin = itemGetPos.position;
            Vector3 direction = itemGetPos.forward;
            RaycastHit[] hits;
            hits = Physics.BoxCastAll(origin, pickupBoxSize / 2, direction, Quaternion.identity, castDistance, pickupMask);
            if(hits.Length <= 0)
            {
                return;
            }

            foreach (RaycastHit hit in hits) 
            {
                //TODO : �Ÿ� ����ؼ� ���� ����� ���������� ����
                adjacentItem = hit.collider.gameObject;
            }

            bucket.OnHideWeapon();
            anim.SetTrigger("IsPickup");
            isPickup = true;
        }
    }

    private void PostPickupItem()
    {
        if (isPickup == true)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Picking Up") == true && stateInfo.normalizedTime > 0.2f && stateInfo.normalizedTime < 0.3f)
            {
                if (adjacentItem == null)
                {
                    return;
                }

                if (adjacentItem.GetComponent<GunData>() != null)
                {
                    bucket.OnRegisterGun(adjacentItem.GetComponent<GunData>());
                }
                adjacentItem.SetActive(false);
                adjacentItem = null;
            }

            if (stateInfo.IsName("Picking Up") == true && stateInfo.normalizedTime >= 0.9f)
            {
                bucket.OnShowWeapon();
                isPickup = false;
            }
        }
    }

    private void UpdateAimTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        aimTarget.position = ray.GetPoint(10.0f);
         
    }
    private void UpdateMouseSet()
    {
        //Mouse Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        pitch = Mathf.Clamp(pitch, -45, 45);
    }

    private void CheckIsGrounded()
    {
        isGround = characterController.isGrounded;
        if (isGround == true && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }
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
        if (Input.GetKeyDown(KeyCode.M) == true)
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
        if(isPickup == true)
        {
            return;
        }

        //Zoom part
        if (Input.GetMouseButtonDown(1) == true)
        {
            if (bucket.CurrentWeapon == null)
            {
                return;
            }

            IsAim = true;
            anim.SetLayerWeight(1, 1); //@tk : ���̾� 1�� ���� 1�� ����

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
            if (bucket.CurrentWeapon == null)
            {
                return;
            }

            IsAim = false;
            anim.SetLayerWeight(1, 0); 

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
        //anim.SetBool("IsAim", isAim);
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
        if(isPickup == false)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //@tk : camTransform = playerHead
            Vector3 direction = camTransform.right * horizontal + camTransform.forward * vertical;
            direction.y = 0; //1��Ī �� �Ʒ� �����̸� ���� �� ��.(�� ����.)
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }       

        //cam ��ġ, 1��Ī ó��
        camTransform.position = playerHead.transform.position;
        camTransform.rotation = Quaternion.Euler(pitch, yaw, 0); //�þ� ������
        transform.rotation = Quaternion.Euler(0f, camTransform.eulerAngles.y, 0); //�� �������� �¿� ȸ���� ����
    }
    private void ThirdPersonMovement()
    {
        if (isPickup == false)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector3 direction = transform.right * horizontal + transform.forward * vertical;
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }


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

            Vector3 destPos = (IsImersion == true) ? playerImmersionLookObj.position : playerLookObj.position;
            camTransform.position = destPos + thirdPersonOffset + rotation * direction;
            camTransform.LookAt(destPos + new Vector3(0, thirdPersonOffset.y, 0));
            UpdateAimTarget();
        }
    }

    private void OnShot()
    {
        if(isAim == false || isShot == true)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0) == true)
        {
            if(bucket.CurrentWeapon == null)
            {
                return;
            }
            if(bucket.CurrentWeapon.CurrentBulletCount <= 0)
            {
                return;
            }

            isShot = true;
            anim.SetTrigger("IsShot");
            bucket.CurrentWeapon.OnShot();
            StartCoroutine(ShotDelayCo());

            float gunMaxRange = CurrentWeapon.CurrentGunData.gunMaxRange;
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            //@tk : multi
            RaycastHit[] hits = Physics.RaycastAll(ray, gunMaxRange, targetMask);
            int searchCount = 0;
            if(hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    searchCount++;
                    Debug.LogFormat($"�浹 ��ü : {hit.collider.gameObject.name}");
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * gunMaxRange, Color.green);
            }

            //@tk : single object
            //RaycastHit hit;
            //if(Physics.Raycast(ray, out hit, gunMaxRange, targetMask))
            //{
            //    if(hit.collider.gameObject.tag.CompareTo("Enemy") == 0)
            //    {
            //        TKZombie zombie = hit.collider.gameObject.GetComponent<TKZombie>();
            //        if(zombie != null)
            //        {
            //            zombie.Damage(bulletDamage);
            //        }

            //        Debug.DrawLine(ray.origin, hit.point, Color.red);
            //    }
            //}
            //else
            //{
            //    Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.green);
            //}
        }
    }

    private void EquippedWeapon()
    {
        if(bucket.WeaponQueue == null || bucket.WeaponQueue.Count == 0)
        {
            return;
        }

        if (IsAim == true || isPickup == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) == true)
        {
            bucket.EquippedWeapon();
            anim.SetTrigger("IsWeaponChange");
        }
    }



    #region On Animation Event
    public void OnAnimEventFootSound()
    {
        //@tk : ���� Raycast�ؼ� �߼Ҹ� ����
        //audioSource.PlayOneShot(audioClipFire);
    }
    public void OnAnimEventWeaponChangeSound()
    {
        audioSource.PlayOneShot(audioClipWeaponChange);
    }

    public void OnAnimEventOneShotSound()
    {
        audioSource.PlayOneShot(audioClipFire);
    }
    #endregion

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

    IEnumerator ShotDelayCo()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < rifleFireDelay) 
        {
            elapsedTime += Time.deltaTime;
            yield return null;  
        }
        isShot = false;
    }
    #endregion

    #region Test
    public void TestCameraShake()
    {
        CameraShake shake = mainCam.GetComponent<CameraShake>();
        if (shake != null)
        {
            shake.ExplosiveCameraShake();
        }
    }

    #endregion
}
