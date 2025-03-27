using UnityEngine;

public class BossChaseState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Run");
        boss.agent.isStopped = false;
        boss.agent.speed = boss.chaseSpeed;
    }

    public void UpdateState(BossManager boss)
    {
        if (boss.Target == null)
        {
            boss.ChangeState(new BossIdleState());
            return;
        }
        
        boss.agent.destination = boss.Target.position;
        if (boss.distanceToTarget < boss.attackRange)
        {
            boss.ChangeState(new BossRoarState());
        }
    }

    public void ExitState(BossManager boss)
    {

    }

}
