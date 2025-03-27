using UnityEngine;

public class BossIdleState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Idle");
        boss.agent.isStopped = true;
    }
    public void UpdateState(BossManager boss)
    {
        if (boss.Target != null)
        {
            boss.ChangeState(new BossChaseState());
        }
    }

    public void ExitState(BossManager boss)
    {

    }

}
