using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System;

public class ZombieManager : MonoBehaviour, IPoolable
{
    public string PoolPath
    {
        get
        {
            return this.GetType().Name;
        }
    }

    public GameObject Prefab
    {
        get
        {
            return gameObject;
        }
    }

    public ZombieState currentState;
    protected List<Transform> patrolPoints;
    protected int currentPointIndex = 0;

    protected Transform target;
    protected CapsuleCollider col;
    protected Rigidbody rigid;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected Coroutine stateRoutine;

    protected float attackRange = 1.0f;
    protected float attackDelay = 1.0f;
    protected float nextAttackTime = 0.0f;
    protected float moveSpeed = 1.0f;
    protected float chaseSpeed = 2.0f;
    protected float evadeSpeed = 3.0f;
    protected float trackingRange = 10.0f;
    protected float evadeRange = 5.0f;
    protected float distanceToTarget;

    //jump
    protected bool isJumping = false;
    protected float jumpHeight = 2.0f;
    protected float jumpDuration = 1.0f;
    protected NavMeshLink[] navMeshLinks;

    protected bool isAttack = false;
    protected bool isWaiting = false; //@tk 상태 전환 후 대기 상태
    protected float idleTime = 2.0f; //@tk 상태 전환 후 대기 시간

    protected GameObject handAttackArea;

    //Health
    protected float zombieHP = 100.0f;

    public ZombieState CurrentState
    {
        get { return currentState; }
        set
        {
            //Debug.LogFormat($"{gameObject.name} : {value} 상태");
            currentState = value;   
        }
    }

    public Transform Target
    {
        get 
        {
            if(target == null || target.gameObject != Operator.Instance.PlayerManager)
            {
                target = Operator.Instance.PlayerManager.transform;
            }
            return target;
        }
    }

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();

        handAttackArea = transform.FindRecursiveChild(Name_Z_AttackArea).gameObject;

        anim.applyRootMotion = false;
    }

    protected void OnEnable()
    {
        if(col.enabled == false)
        {
            col.enabled = true;
        }

        navMeshLinks = FindObjectsOfType<NavMeshLink>();

        distanceToTarget = Vector3.Distance(transform.position, Target.position);
        CurrentState = ZombieState.Idle;
        stateRoutine = StartCoroutine(currentState.ToString());
    }

    private void Start()
    {
        patrolPoints = new List<Transform>();
        foreach (Transform patrolPoint in Operator.Instance.PatrolManager.GetRandomPointList())
        {
            patrolPoints.Add(patrolPoint);
        }
    }

    private void Update()
    {
        if (currentState == ZombieState.Die) 
        {
            return;
        }

        if(isJumping == true)
        {
            return;
        }

        distanceToTarget = Vector3.Distance(transform.position, Target.position);
    }

    public void ChangeState(ZombieState state)
    {
        if (currentState == ZombieState.Die)
        {
            return;
        }
        if (isJumping == true)
        {
            return;
        }
        if (currentState == state)
        {
            return;
        }

        if(stateRoutine != null)
        {
            StopCoroutine(stateRoutine);
        }

        CurrentState = state;
        stateRoutine = StartCoroutine(state.ToString());
    }

    private void SetStateByDistance()
    {
        if (distanceToTarget < trackingRange)
        {
            if (distanceToTarget < attackRange)
            {
                ChangeState(ZombieState.Attack);
            }
            else
            {
                ChangeState(ZombieState.Chase); 
            }
        }
        
        else
        {
            if (patrolPoints == null || patrolPoints.Count <= 0) 
            {
                ChangeState(ZombieState.Idle);
            }
            else
            {
                ChangeState(ZombieState.Patrol);
            }
        }
    }

    public void OnDamage(float damage) 
    {
        if(currentState == ZombieState.Die)
        {
            return;
        }

        if(stateRoutine != null)
        {
            StopCoroutine(stateRoutine);
        }

        currentState = ZombieState.Damaged;
        stateRoutine = StartCoroutine(Damaged(damage));
    }


    public IEnumerator Patrol()
    {
        anim.SetBool("IsWalk", true);
        anim.SetBool("IsRun", false);
        while (currentState == ZombieState.Patrol) 
        {
            SetStateByDistance();
            Transform targetPoint = patrolPoints[currentPointIndex];
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            //transform.position += direction * moveSpeed * Time.deltaTime;
            //transform.LookAt(targetPoint.position);
            agent.speed = moveSpeed;
            agent.isStopped = false;
            agent.destination = targetPoint.position;

            if (agent.isOnOffMeshLink == true)
            {
                //TODO
                StartCoroutine(JumpAcrossLink());
            }


            if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            }

            yield return null;
        }
    }
    public IEnumerator Chase()
    {
        anim.SetBool("IsRun", true);
        while (currentState == ZombieState.Chase)
        {
            //transform.LookAt(Target.position);
            SetStateByDistance();
            
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if(stateInfo.IsName("Z_Scream") && stateInfo.normalizedTime < 1.0f)
            {
                //TODO
            }
            else if (stateInfo.IsName("Z_Attack_s") && stateInfo.normalizedTime < 1.0f)
            {
                //TODO
            }
            else if(stateInfo.IsName("Z_Damaged") && stateInfo.normalizedTime < 1.0f)
            {
                //TODO
            }
            else
            {
                Vector3 direction = (Target.position - transform.position).normalized;
                agent.speed = chaseSpeed;
                agent.isStopped = false;
                agent.destination = target.position;
            }
            
            yield return null;
        }
    }
    public IEnumerator Idle()
    {
        anim.Play("Z_Idle");
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsRun", false);

        agent.speed = 0f;
        agent.isStopped = false;

        while (currentState == ZombieState.Idle)
        {
            SetStateByDistance();
            yield return null;
        }
    }

    public IEnumerator Attack()
    {
        while (currentState == ZombieState.Attack)
        {
            SetStateByDistance();

            if (distanceToTarget < attackRange)
            {
                agent.speed = 0.0f;
                agent.isStopped = true;
                
                if(transform.IsTargetInFront(target) == false)
                {
                    agent.speed = chaseSpeed;
                    agent.destination = target.position;
                    agent.isStopped = false;
                }
                
                anim.SetTrigger("OnAttack_s");
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }
    public IEnumerator Die()
    {
        //TODO : 일부 수정 필요
        anim.SetTrigger("OnDie");

        if (col.enabled == true)
        {
            col.enabled = false;
        }

        agent.speed = 0f;
        agent.isStopped = true;
        
        

        yield return new WaitForSeconds(2.0f);
        PoolManager.Instance.DeSpawnObject(this);
    }
    //@tk 외부에서 호출
    public IEnumerator Damaged(float damage)
    {
        //TODO : 계속 못 맞게 무적
        zombieHP -= damage;
        if(zombieHP <= 0)
        {
            ChangeState(ZombieState.Die);
        }
        else
        {
            anim.SetTrigger("OnDamaged");
            Debug.LogFormat($"zombie HP : {zombieHP}");
            agent.speed = 0f;
            agent.isStopped = true;

            if (distanceToTarget < trackingRange)
            {
                ChangeState(ZombieState.Chase); 
            }
            else
            {
                //@tk: Evade는 나중에 다른 로직으로
                ChangeState(ZombieState.Patrol);
            }
        }
        yield return null;
    }
    public IEnumerator Evade()
    {
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsRun", true);

        Vector3 evadeDirection = (transform.position - Target.position).normalized;
        float evadeTime = 3.0f;
        float timer = 0.0f;

        agent.speed = evadeSpeed;
        agent.isStopped = true;
        agent.destination = transform.position + evadeDirection * 10f;

        while (currentState == ZombieState.Evade && timer < evadeTime)
        {
            if(Vector3.Distance(agent.destination, transform.position) < 1.2f)
            {
                ChangeState(ZombieState.Idle);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        ChangeState(ZombieState.Idle);
    }

    //@tk : NavMeshLink 이동
    //@Patrol 중에 행동 코루틴이라서 상태 아님.
    /*
    해당 코드 발전 방향
    -> 애니메이션 싱크 맞추기
    -> 혹은, 기어가는 애니메이션 맞추기
     */
    public IEnumerator JumpAcrossLink()
    {
        Debug.Log($"{gameObject.name} : 점프");
        isJumping = true;
        agent.isStopped = true;

        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = linkData.startPos;
        Vector3 endPos = linkData.endPos;

        float elapsedTime = 0f;
        while(elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        isJumping = false;
    }

    public void OnSfxByState(ZombieState state)
    {
        switch (state)
        {
            case ZombieState.Patrol:
                break;
            case ZombieState.Chase:
                break;
            case ZombieState.Attack:
                if(distanceToTarget < attackRange)
                {
                    SoundManager.Instance.PlaySFX("SFX_Zombie_Attack_s");
                }
                break;
            case ZombieState.Evade:
                break;
            case ZombieState.Damaged:
                break;
            case ZombieState.Idle:
                break;
            case ZombieState.Die:
                break;
            default:
                break;
        }
    }

    public void OnSfxScream()
    {
        //@tk 이거 소리 다른걸로 바꿔야함. 너무 커서 주석처리
        //audioSource.PlayOneShot(sfx_scream);
    }

    private readonly string Name_Z_AttackArea = "@AttackArea";
}
