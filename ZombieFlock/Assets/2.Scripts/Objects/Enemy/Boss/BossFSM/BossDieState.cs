using UnityEngine;

public class BossDieState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Die");

    }

    public void ExitState(BossManager boss)
    {

    }

    public void UpdateState(BossManager boss)
    {

    }
}
