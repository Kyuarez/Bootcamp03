using UnityEngine;

public interface IBossState
{
    void EnterState(BossManager boss); //Anim, value setting
    void UpdateState(BossManager boss); //Logic
    void ExitState(BossManager boss); //
}