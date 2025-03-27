using UnityEngine;

public class BossRoarState : IBossState
{
    private bool isOn = false;

    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Roar");
        boss.agent.isStopped = true;
        SoundManager.Instance.PlaySFX("SFX_BlackBull_Grawl");
    }
    public void UpdateState(BossManager boss)
    {
        if(isOn == true)
        {
            return;
        }

        AnimatorStateInfo info = boss.GetBossAnimStateInfo();
        string animName = $"{boss.ObjectData.CodeName}_Roar";
        if(info.IsName(animName) == true && info.normalizedTime >= 0.9f)
        {
            int rndNum = UnityEngine.Random.Range(0, 10);
            if(rndNum <= 2) // 30%
            {
                boss.ChangeState(new BossRushState());

                //boss.ChangeState(new BossJumpAttackState());
            }
            else
            {
                boss.ChangeState(new BossRushState());
            }

            isOn = true;
        }
    }

    public void ExitState(BossManager boss)
    {
        isOn = false;
    }

}
