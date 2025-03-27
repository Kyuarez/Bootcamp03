using UnityEngine;

public class BossRushState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Rush");
    }

    public void ExitState(BossManager boss)
    {

    }

    public void UpdateState(BossManager boss)
    {

    }
}
