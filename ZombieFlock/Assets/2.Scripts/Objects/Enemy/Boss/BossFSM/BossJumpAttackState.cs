using UnityEngine;

public class BossJumpAttackState : IBossState
{
    private bool isOn = false;
    private Vector3 jumpTarget; 
    private float jumpHeight = 5.0f; 

    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("JumpAttack");
        boss.agent.enabled = false;
        boss.agent.isStopped = true; 
        jumpTarget = boss.Target.position;
    }
    public void UpdateState(BossManager boss)
    {
        if (isOn == true)
        {
            return;
        }

        Vector3 currentPosition = boss.transform.position;
        Vector3 direction = (jumpTarget - currentPosition).normalized;

        float step = boss.rushSpeed * Time.deltaTime;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, jumpTarget + Vector3.up * jumpHeight, step);
        boss.transform.position = nextPosition;

        AnimatorStateInfo info = boss.GetBossAnimStateInfo();
        string animName = $"{boss.ObjectData.CodeName}_JumpAttack";
        if (info.IsName(animName) == true && info.normalizedTime >= 0.9f)
        {
            isOn = true;
            boss.ChangeState(new BossIdleState());
        }
    }

    public void ExitState(BossManager boss)
    {
        isOn = false;
        boss.agent.enabled = true;
    }

}
