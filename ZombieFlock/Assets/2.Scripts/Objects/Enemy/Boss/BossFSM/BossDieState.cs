using UnityEngine;

public class BossDieState : IBossState
{
    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Die");
        boss.isDie = true;
        boss.agent.isStopped = true;
        boss.agent.enabled = false;

        string sfxString = $"sfx_{boss.ObjectData.CodeName}_Die";
        SoundManager.Instance.PlaySFX(sfxString, boss.transform.position);
    }
    public void UpdateState(BossManager boss)
    {

    }

    public void ExitState(BossManager boss)
    {

    }

}
