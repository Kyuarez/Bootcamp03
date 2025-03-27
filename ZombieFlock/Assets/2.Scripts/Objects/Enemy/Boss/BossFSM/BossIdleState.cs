using UnityEngine;

public class BossIdleState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Idle");
    }

    public void ExitState(BossManager boss)
    {

    }

    public void UpdateState(BossManager boss)
    {

    }
}
