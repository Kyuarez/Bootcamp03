using UnityEngine;

public class BossJumpAttackState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("JumpAttack");
    }

    public void ExitState(BossManager boss)
    {

    }

    public void UpdateState(BossManager boss)
    {

    }
}
