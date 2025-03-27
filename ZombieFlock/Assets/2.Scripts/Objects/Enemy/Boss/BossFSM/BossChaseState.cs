using UnityEngine;

public class BossChaseState : IBossState
{
    protected float chaseSpeed = 5.0f;

    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Run");
    }

    public void UpdateState(BossManager boss)
    {
        if (boss.Target == null)
            return;

        boss.agent.destination = boss.Target.position;

        float distance = Vector3.Distance(boss.transform.position, boss.Target.position);

        if (distance < boss.attackRange)
        {
            boss.ChangeState(new BossJumpAttackState());
        }
    }

    public void ExitState(BossManager boss)
    {

    }

}
