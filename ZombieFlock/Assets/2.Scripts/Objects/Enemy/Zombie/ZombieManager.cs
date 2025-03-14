using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
    protected Animator anim;
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

    protected bool isAttack = false;
    protected bool isWaiting = false; //@tk 상태 전환 후 대기 상태
    protected float idleTime = 2.0f; //@tk 상태 전환 후 대기 시간

    //Health
    protected float zombieHP = 100.0f;

    //Sound : 나중엔 매니저에서 일괄 처리 클립 가지고 있는거 불편함
    [Header("Sound")]
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip sfx_attack_s;
    [SerializeField] protected AudioClip sfx_attack_b;
    [SerializeField] protected AudioClip sfx_dead;
    [SerializeField] protected AudioClip sfx_scream;


    public ZombieState CurrentState
    {
        get { return currentState; }
        set
        {
            Debug.LogFormat($"{gameObject.name} : {value} 상태");
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
        audioSource = GetComponent<AudioSource>();

        anim.applyRootMotion = false;
    }

    protected void OnEnable()
    {
        distanceToTarget = Vector3.Distance(transform.position, Target.position);
        CurrentState = ZombieState.Idle;
        stateRoutine = StartCoroutine(currentState.ToString());

        patrolPoints = Operator.Instance.PatrolManager.GetRandomPointList();
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, Target.position);
    }

    public void ChangeState(ZombieState state)
    {
        if(currentState == state)
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
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(targetPoint.position);

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
            transform.LookAt(Target.position);
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
                transform.position += direction * chaseSpeed * Time.deltaTime;
            }
            
            yield return null;
        }
    }
    public IEnumerator Idle()
    {
        anim.Play("Z_Idle");
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsRun", false);
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

            if(distanceToTarget < attackRange)
            {
                anim.SetTrigger("OnAttack_s");
                transform.LookAt(target.position);
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }
    public IEnumerator Die()
    {
        //TODO : 일부 수정 필요
        anim.SetTrigger("OnDie");
        yield return new WaitForSeconds(2.0f);
        PoolManager.Instance.DeSpawnObject(this);
    }
    //@tk 외부에서 호출
    public IEnumerator Damaged(float damage)
    {
        //TODO : 계속 못 맞게 무적
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Z_Damaged") == true)
        {
            while(stateInfo.normalizedTime < 1.0f)
            {
                yield return null;
            }
        }

        anim.SetTrigger("OnDamaged");
        zombieHP -= damage;
        Debug.LogFormat($"zombie HP : {zombieHP}");

        if(zombieHP <= 0)
        {
            ChangeState(ZombieState.Die);
        }
        else
        {
            if(distanceToTarget < trackingRange)
            {
                ChangeState(ZombieState.Chase); 
            }
            else
            {
                ChangeState(ZombieState.Evade);
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

        Quaternion targetRotation = Quaternion.LookRotation(evadeDirection);
        transform.rotation = targetRotation;

        while (currentState == ZombieState.Evade && timer < evadeTime)
        {
            transform.position += evadeDirection * evadeSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        ChangeState(ZombieState.Idle);
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
                    audioSource.PlayOneShot(sfx_attack_s);
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
}
