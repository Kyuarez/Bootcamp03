using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    public ObjectData ObjectData;
    public NavMeshAgent agent;
    public GameObject rushAttackArea;

    protected Transform target;
    protected CapsuleCollider col;
    protected Rigidbody rigid;
    protected Animator anim;
    protected Coroutine cutsceneCoroutine;

    public float attackRange = 5.0f;
    public float moveSpeed = 1.0f;
    public float chaseSpeed = 5.0f;
    public float rushSpeed = 20.0f;
    public float trackingRange = 10.0f;
    public float distanceToTarget;

    protected NavMeshLink[] navMeshLinks;

    protected float zombieHP = 1000.0f;

    protected IBossState currentState;

    public static event Action<int, int> OnDie; //@tk 모든 몬스터 공유

    public bool isDie = false;

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

        if(cutsceneCoroutine != null)
        {
            StopCoroutine(cutsceneCoroutine);
            cutsceneCoroutine = null;
        }

        agent.enabled = false;
        transform.LookAt(Target);
        agent.enabled = true;
        cutsceneCoroutine = StartCoroutine(CutSceneCo());
    }

    protected void OnDisable()
    {
        currentState = null;
    }

    private void Update()
    {
        if(isDie == true)
        {
            return;
        }

        if (Target == null)
        {
            return;
        }

        distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if (currentState != null)
        {
            currentState.UpdateState(this);
        }

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
        string animName = ObjectData.CodeName + "_" + state;
        anim.Play(animName);
    }

    public AnimatorStateInfo GetBossAnimStateInfo(int layer = 0)
    {
        return anim.GetCurrentAnimatorStateInfo(layer);
    }
    public void OnDamaged(float damage)
    {
        zombieHP = Mathf.Clamp(zombieHP - damage, 0, zombieHP);
        Debug.Log($"{ObjectData.CodeName}'s  HP : {zombieHP}");
        //TODO : 0 이하면 죽는 State
        if(zombieHP <= 0)
        {
            //TODO.
            ChangeState(new BossDieState());
            OnDie?.Invoke(ObjectData.ObjectID, 1);
        }
    }

    protected IEnumerator CutSceneCo()
    {
        PlayAnimation("Idle");
        yield return new WaitForSeconds(4f);
        PlayAnimation("Roar");
        SoundManager.Instance.PlaySFX("SFX_BlackBull_Grawl");
        yield return new WaitForSeconds(1f);
        distanceToTarget = Vector3.Distance(transform.position, Target.position);
        ChangeState(new BossIdleState());
    }
}
