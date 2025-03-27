using UnityEngine;

public class BossJumpAttackState : IBossState
{
    private bool isOn = false;

    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("JumpAttack");
        boss.agent.isStopped = false;
    }
    public void UpdateState(BossManager boss)
    {
        if (isOn == true)
        {
            return;
        }

        boss.agent.destination = boss.Target.position;
        AnimatorStateInfo info = boss.GetBossAnimStateInfo();
        string animName = $"{boss.ObjectData.CodeName}_JumpAttack";
        if (info.IsName(animName) == true && info.normalizedTime >= 0.9f)
        {
            isOn = true;
        }
    }

    public void ExitState(BossManager boss)
    {
        isOn = false;

    }

}
