using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TKCamera;
using UnityEngine.Animations.Rigging;
using static UnityEngine.UI.Image;

public class PlayerManager : MonoBehaviour
{
    public float walkSpeed = 2.5f;
    public float runSpeed = 5.0f;
    public float mouseSensitivity = 100.0f; //마우스 감도
    private float moveSpeed;
    private float currentRecoil;
    public LayerMask targetMask;

    private Transform camTransform;
    private Transform playerHead; //플레이어 머리 위치(1인칭 cam)
    private Transform WeaponTransform;
    public float thirdPersonDistance = 3.0f; //플레이어 - cam 거리
    public float immersionDistance = 1.0f; //3인칭 몰입형 거리
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.5f, 0f);
    private Transform playerLookObj; //플레이어 시야 위치 (배그 숄더숏)
    private Transform playerImmersionLookObj; //플레이어 시야 위치 : 다리 자르기

    //Sight
    public float zoomDistance = 1.0f; //3인칭
    public float zoomImmersionDistance = 0.5f;
    public float zoomSpeed = 5.0f;
    public float defaultFov = 60.0f;
    public float zoomFov = 30.0f; //확대 시 카메라 시야각 (1인칭)

    private float currentDistance; //현재 카메라와의 거리 (3인칭)
    private float targetDistance; //목표 카메라 거리
    private float targetFov;
    private Coroutine zoomCoroutine; //코루틴 사용하여 확대 축소 처리
    private Camera mainCam;

    private float pitch = 0.0f; //위-아래 (인사)
    private float yaw = 0.0f; //좌우 회전값
    private bool isFirstPerson = false; //1인칭 모드 여부
    private bool isImmersion = false; //몰입 모드 여부
    private bool isRotaterAroundPlayer = true; //카메라가 플레이어 주위를 회전하는지 여부

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
    private Transform aimTarget;
    public MultiAimConstraint multiAimConstraint;

    //ItemPickup
    private Vector3 pickupBoxSize = new Vector3(1.0f, 1.0f, 1.0f);
    private float castDistance = 5.0f;
    public LayerMask pickupMask;
    private Transform itemGetPos;

    private bool isShot = false;
    private BucketManager bucket;

    //@tk particle
    private GameObject flashLight;

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

    private void Awake()
    {
        playerHead = transform.FindRecursiveChild(Name_PlayerHead);
        WeaponTransform = transform.FindRecursiveChild(Name_WeaponTransform);
        playerLookObj = transform.FindRecursiveChild(Name_PlayerObj);
        playerImmersionLookObj = transform.FindRecursiveChild(Name_PlayerImmersionObj);
        aimTarget = transform.FindRecursiveChild(Name_AimTarget);
        itemGetPos = transform.FindRecursiveChild(Name_PickupTransform);
        flashLight = transform.FindRecursiveChild(Name_FlashLight).gameObject;
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

        flashLight.SetActive(false);
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
        OnReload();
        PickupItem();
        PostPickupItem();

        //recoil
        if (currentRecoil > 0f)
        {
            currentRecoil -= bucket.CurrentWeapon.CurrentGunData.recoilMagnitude * Time.deltaTime;
            currentRecoil = Mathf.Clamp(currentRecoil, 0, bucket.CurrentWeapon.CurrentGunData.recoilAngle);
            Quaternion currentRotation = Camera.main.transform.rotation;
            Quaternion recoliRotation = Quaternion.Euler(-currentRecoil, 0, 0);
            Camera.main.transform.rotation = currentRotation * recoliRotation;
        }
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

            //TODO : 아이템 체킹해서 예외 처리
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
                //TODO : 거리 계산해서 가장 가까운 아이템으로 세팅
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

        //1인칭, 3인칭 결정
        if (Input.GetKeyDown(KeyCode.V) == true)
        {
            isFirstPerson = !isFirstPerson;
        }
        //플레이어 주변 자동 회전 여부
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            isRotaterAroundPlayer = !isRotaterAroundPlayer;
        }
        //몰입형 모드인지 여부
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
            anim.SetLayerWeight(1, 1); //@tk : 레이어 1번 무게 1로 변경

            //Coroutine 관리 위해서, 변수화(메소드 명 직접 받으면 이렇게 관리)
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

            if(flashLight.activeSelf == false)
            {
                flashLight.SetActive(true);
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

            if(flashLight.activeSelf == true)
            {
                flashLight.SetActive(false);
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
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if(isPickup == false && stateInfo.IsName("Rifle Pull Out") == false && stateInfo.IsName("Damaged") == false)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //@tk : camTransform = playerHead
            Vector3 direction = camTransform.right * horizontal + camTransform.forward * vertical;
            direction.y = 0; //1인칭 위 아래 움직이면 절대 안 됨.(눈 아파.)
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }       

        //cam 위치, 1인칭 처리
        camTransform.position = playerHead.transform.position;
        camTransform.rotation = Quaternion.Euler(pitch, yaw, 0); //시야 움직임
        transform.rotation = Quaternion.Euler(0f, camTransform.eulerAngles.y, 0); //몸 움직임은 좌우 회전만 강제
    }
    private void ThirdPersonMovement()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if ((isPickup == false && stateInfo.IsName("Rifle Pull Out") == false) && stateInfo.IsName("Damaged") == false)
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

            Vector3 destPos = (IsImersion == true) ? playerImmersionLookObj.position : playerLookObj.position;
            camTransform.position = destPos + thirdPersonOffset + rotation * direction;
            camTransform.LookAt(destPos + new Vector3(0, thirdPersonOffset.y, 0));
            UpdateAimTarget();
        }
    }

    private void OnShot()
    {
        if (isAim == false || isShot == true)
        {
            return;
        }
        if (bucket.CurrentWeapon == null)
        {
            return;
        }

        switch (bucket.CurrentWeapon.CurrentGunType)
        {
            case GunType.Rifle:
                OnRifleShot();
                break;
            case GunType.Sniper:
                OnSniperShot();
                break;
            case GunType.Shotgun:
                OnShotgunShot();    
                break;
            default:
                break;
        }

    }

    private void OnRifleShot()
    {
        if (Input.GetMouseButton(0) == true)
        {
            if (bucket.CurrentWeapon.CurrentBulletCount <= 0)
            {
                return;
            }

            if(isShot == true)
            {
                return;
            }

            isShot = true;
            anim.ResetTrigger("IsShot");
            anim.SetTrigger("IsShot");
            bucket.CurrentWeapon.OnShot();
            SoundManager.Instance.PlaySFX("SFX_Weapon_Rifle", transform.position);
            StartCoroutine(ShotDelayCo(bucket.CurrentWeapon.CurrentGunData.shotDelay));

            float gunMaxRange = CurrentWeapon.CurrentGunData.gunMaxRange;
            RaycastHit hit;
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            if(Physics.Raycast(ray, out hit, gunMaxRange, targetMask))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    ZombieManager zombie = hit.collider.GetComponent<ZombieManager>();
                    if (zombie != null)
                    {
                        zombie.OnDamage(bucket.CurrentWeapon.CurrentGunData.gunDamage);
                        ParticleManager.Instance.PlayFX(EffectType.FX_RiflingMark_SoftBody, hit.point);
                        //PoolManager.Instance.SpawnObjectInWorld<FX_RiflingMark_SoftBody>(hit.point);
                        SoundManager.Instance.PlaySFX("SFX_Zombie_Damaged", zombie.transform.position);
                        Debug.DrawLine(ray.origin, hit.point, Color.red);

                    }
                    else
                    {
                        Debug.DrawLine(ray.origin, ray.origin + ray.direction * gunMaxRange, Color.green);
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    //@tk 이거 환경 재질 마다 차이 주기
                    ParticleManager.Instance.PlayFX(EffectType.FX_RiflingMark_Concrete, hit.point);
                    //PoolManager.Instance.SpawnObjectInWorld<FX_RiflingMark_Concrete>(hit.point);
                }

            }
        }
    }

    private void OnSniperShot()
    {

    }

    private void OnShotgunShot()
    {   
        if (Input.GetMouseButtonDown(0) == true)
        {
            if (bucket.CurrentWeapon.CurrentBulletCount <= 0)
            {
                return;
            }

            isShot = true;
            anim.SetTrigger("IsShot");
            bucket.CurrentWeapon.OnShot();
            SoundManager.Instance.PlaySFX("SFX_Weapon_Shotgun", transform.position);
            StartCoroutine(ShotDelayCo(bucket.CurrentWeapon.CurrentGunData.shotDelay));


            //@tk : shotgun (한번에 5탄 Ray로 Random하게 쏴서 중복 데미지 적용)
            int shotCount = 5;
            float spreadAngle = 15f;
            float gunMaxRange = CurrentWeapon.CurrentGunData.gunMaxRange;

            for (int i = 0; i < shotCount; i++)
            {
                float randomX = Random.Range(-spreadAngle, spreadAngle);
                float randomY = Random.Range(-spreadAngle, spreadAngle);
                Vector3 randDirection = (new Vector3(randomX, randomY, 0f).normalized * 0.1f) + mainCam.transform.forward;
                Ray ray = new Ray(mainCam.transform.position, randDirection);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, gunMaxRange, targetMask))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        ZombieManager zombie = hit.collider.GetComponent<ZombieManager>();
                        if (zombie != null)
                        {
                            zombie.OnDamage(bucket.CurrentWeapon.CurrentGunData.gunDamage);
                            ParticleManager.Instance.PlayFX(EffectType.FX_RiflingMark_SoftBody, hit.point);
                            //PoolManager.Instance.SpawnObjectInWorld<FX_RiflingMark_SoftBody>(hit.point);
                            SoundManager.Instance.PlaySFX("SFX_Zombie_Damaged", zombie.transform.position);
                            Debug.DrawLine(ray.origin, hit.point, Color.red);

                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + ray.direction * gunMaxRange, Color.green);
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                    {
                        //@tk 이거 환경 재질 마다 차이 주기
                        ParticleManager.Instance.PlayFX(EffectType.FX_RiflingMark_Concrete, hit.point);
                        //PoolManager.Instance.SpawnObjectInWorld<FX_RiflingMark_Concrete>(hit.point);
                    }

                }
            }
        }
    }

    private void ApplyRecoil()
    {
        Quaternion currentRotation = Camera.main.transform.rotation;
        Quaternion recoilRotation = Quaternion.Euler(-currentRecoil, 0, 0);
        Camera.main.transform.rotation = currentRotation * recoilRotation;
        currentRecoil += bucket.CurrentWeapon.CurrentGunData.recoilMagnitude;
        currentRecoil = Mathf.Clamp(currentRecoil, 0, bucket.CurrentWeapon.CurrentGunData.recoilAngle);
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

    private void OnReload()
    {
        if (bucket.CurrentWeapon == null)
        {
            return;
        }
        if (IsAim == true || isPickup == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            if (bucket.CurrentWeapon.OnReloading() == true)
            {
                anim.SetTrigger("IsWeaponChange");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyAttack") == true)
        {
            anim.SetTrigger("OnDamaged");
            
        }
    }


    #region On Animation Event
    public void OnAnimEventFootSound()
    {

    }
    public void OnAnimEventWeaponChangeSound()
    {
        SoundManager.Instance.PlaySFX("SFX_Weapon_Equipped", transform.position);
    }

    public void OnAnimEventOneShotSound()
    {
        //
    }
    #endregion

    #region Coroutine
    /// <summary>
    /// 3인칭 Zoom : distance Lerp
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
    /// 1인칭 Zoom : fov 변경
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

    IEnumerator ShotDelayCo(float delay)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < delay) 
        {
            elapsedTime += Time.deltaTime;
            yield return null;  
        }
        isShot = false;
        CurrentWeapon.OnShotFX(false);
    }
    #endregion

    #region Gizmos
    


    #endregion

    private readonly string Name_PlayerHead = "head";
    private readonly string Name_WeaponTransform = "@WeaponTransform";
    private readonly string Name_PlayerObj = "@PlayerObj";
    private readonly string Name_PlayerImmersionObj = "@PlayerImmersionObj";
    private readonly string Name_AimTarget = "@AimTarget";
    private readonly string Name_PickupTransform = "@PickupTransform";
    private readonly string Name_FlashLight = "@FlashLight";
}
