using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public ZombieState currentState = ZombieState.Idle;
    public Transform[] patrolPoints;
    protected int currentPointIndex = 0;

    protected Transform target;
    protected Animator anim;
    
    protected float attackRange = 1.0f;
    protected float attackDelay = 2.0f;
    protected float nextAttackTime = 0.0f;
    protected float moveSpeed = 1.0f;
    protected float chaseSpeed = 2.0f;
    protected float trackingRange = 5.0f;
    protected float evadeRange = 5.0f;
    protected float distanceToTarget;

    protected bool isAttack = false;
    protected bool isWaiting = false; //@tk 상태 전환 후 대기 상태
    protected float idleTime = 2.0f; //@tk 상태 전환 후 대기 시간

    //Health
    protected float zombieHP = 10.0f;

    public ZombieState CurrentState
    {
        get { return currentState; }
        set
        {
            if(currentState == value)
            {
                return;
            }

            SetAnimByState(value);
            currentState = value;
        }
    }

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
    }

    protected void OnEnable()
    {
        target = Operator.Instance.PlayerManager?.transform;
    }

    protected void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);
        if(distanceToTarget < trackingRange)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Z_Scream") && stateInfo.normalizedTime <= 0.95)
            {
                return;
            }

            if (distanceToTarget < attackRange)
            {
                CurrentState = ZombieState.Attack;
                return;
            }

            CurrentState = ZombieState.Chase;
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * chaseSpeed * Time.deltaTime;
            transform.LookAt(target);
        }
        else
        {
            if(patrolPoints.Length > 0)
            {
                CurrentState = ZombieState.Patrol;
                Transform targetPoint = patrolPoints[currentPointIndex];
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(targetPoint);

                if(Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
                {
                    currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                }
                return;
            }
            else
            {
                CurrentState = ZombieState.Idle;    
            }

        }
    }

    public void SetAnimByState(ZombieState state)
    {
        switch (state)
        {
            case ZombieState.Patrol:
                anim.SetBool("IsPatrol", true);
                anim.SetBool("IsChase", false);
                break;
            case ZombieState.Chase:
                anim.SetBool("IsChase", true);
                break;
            case ZombieState.Attack:
                break;
            case ZombieState.Evade:
                break;
            case ZombieState.Damage:
                break;
            case ZombieState.Idle:
                anim.SetBool("IsPatrol", false);
                anim.SetBool("IsChase", false);
                break;
            case ZombieState.Die:
                break;
            default:
                break;
        }
    }
}
