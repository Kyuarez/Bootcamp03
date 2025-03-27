using UnityEngine;

public class BossRushState : IBossState
{
    private bool isOn = false;
    private Vector3 rushTarget;

    public void EnterState(BossManager boss)
    {
        boss.PlayAnimation("Rush");
        boss.rushAttackArea.SetActive(true);

        Vector3 direction = (boss.Target.position - boss.transform.position).normalized;
        rushTarget = boss.transform.position + direction * 30f;

        boss.agent.isStopped = false;
        boss.agent.speed = boss.rushSpeed;
        boss.agent.destination = rushTarget;
    }
    public void UpdateState(BossManager boss)
    {
        if(isOn == true)
        {
            return;
        }

        // 목표 위치에 도달했는지 확인
        if (!boss.agent.pathPending && boss.agent.remainingDistance <= boss.agent.stoppingDistance)
        {
            isOn = true;
            boss.ChangeState(new BossIdleState());
        }
    }

    public void ExitState(BossManager boss)
    {
        isOn = false;
        boss.agent.speed = boss.chaseSpeed;
        boss.rushAttackArea.SetActive(false);
    }

}
