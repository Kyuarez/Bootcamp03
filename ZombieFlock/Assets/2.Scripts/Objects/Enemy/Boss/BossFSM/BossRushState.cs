using UnityEngine;

public class BossRushState : IBossState
{
    private bool isOn = false;

    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Rush");
        boss.agent.isStopped = false;
        boss.agent.speed = boss.chaseSpeed;
    }
    public void UpdateState(BossManager boss)
    {
        if(isOn == true)
        {
            return;
        }

        boss.agent.destination = boss.Target.position;
        if(boss.distanceToTarget < 1.8f)
        {
            isOn = true;
            boss.ChangeState(new BossIdleState());
        }
    }

    public void ExitState(BossManager boss)
    {
        isOn = false;
        boss.agent.speed = boss.chaseSpeed;
    }

}
