using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    public ObjectData ObjectData;
    public NavMeshAgent agent;

    protected Transform target;
    protected CapsuleCollider col;
    protected Rigidbody rigid;
    protected Animator anim;
    protected Coroutine stateRoutine;

    public float attackRange = 1.0f;
    public float attackDelay = 1.0f;
    public float nextAttackTime = 0.0f;
    public float moveSpeed = 1.0f;
    public float chaseSpeed = 5.0f;
    public float trackingRange = 10.0f;
    public float distanceToTarget;

    protected NavMeshLink[] navMeshLinks;

    protected float zombieHP = 1000.0f;

    protected IBossState currentState;

    public static event Action<int, int> OnDie; //@tk 모든 몬스터 공유


    public Transform Target
    {
        get
        {
            if (Operator.Instance.PlayerManager == null)
            {
                return null;
            }

            if (target == null || target.gameObject != Operator.Instance.PlayerManager)
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

        anim.applyRootMotion = false;
    }

    protected void OnEnable()
    {
        if (col.enabled == false)
        {
            col.enabled = true;
        }

        navMeshLinks = FindObjectsOfType<NavMeshLink>();

        if (Target == null)
        {
            return;
        }

        //CurrentState = ZombieState.Idle;
        //stateRoutine = StartCoroutine(currentState.ToString());
        distanceToTarget = Vector3.Distance(transform.position, Target.position);

        //ChangeState(new BossIdleState());
    }

    protected void OnDisable()
    {
        currentState = null;
    }

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        distanceToTarget = Vector3.Distance(transform.position, Target.position);
        agent.speed = chaseSpeed;
        agent.isStopped = false;

        agent.destination = target.position;

        //if (currentState != null)
        //{
        //    currentState.UpdateState(this);
        //}

    }

    public void ChangeState(IBossState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void PlayAnimation(string state)
    {
        anim.Play(ObjectData.CodeName + "_" + state.ToString());
    }

    public void OnDamaged(float damage)
    {
        zombieHP = Mathf.Clamp(zombieHP - damage, 0, zombieHP);
        Debug.Log($"{ObjectData.CodeName}'s  HP : {zombieHP}");
        //TODO : 0 이하면 죽는 State
        if(zombieHP <= 0)
        {
            //TODO.
            string sfxString = $"sfx_{ObjectData.CodeName}_Die";
            SoundManager.Instance.PlaySFX(sfxString, transform.position);
            OnDie?.Invoke(ObjectData.ObjectID, 1);
            Destroy(gameObject ,1.0f);
        }
    }
}
