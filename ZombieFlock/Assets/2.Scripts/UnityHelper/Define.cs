using UnityEngine;

public static class Define
{
    public static Vector3 Rifle_Pos = new Vector3(0.776f, -0.249f, 0.54f);
    public static Vector3 Rifle_Rotate = new Vector3(-3.946f, 26.312f, 75f);
    public static Vector3 RifleAim_Pos = new Vector3(1.034f, -0.258f, -0.163f);
    public static Vector3 RifleAim_Rotate = new Vector3(14.174f, 101.724f, 78f);


    public const string RES_WEAPONS = "Prefabs/Weapon";
    public const string RES_SO_GUN = "SO/Weapon";

    public static readonly string Local_DataPath = Application.streamingAssetsPath + "/Local";
}

#region Enum
public enum GameState
{
    Title,
    InGame,
}

public enum GunType
{
    Rifle,
    Sniper,
    Shotgun,
}

public enum PickableType
{
    Gun,
    Usable,
}

public enum ZombieState
{
    Patrol,
    Chase,
    Attack,
    Evade,
    Damaged,
    Idle,
    Die,
}

public enum EffectType
{
    FX_RiflingMark_SoftBody,
    FX_RiflingMark_Concrete,
}

public enum QuestConditionType
{
    GetItem,
    Kill,
    ActiveEvent,
}

public enum ActiveEventType //Quest 조건 : 특정 이벤트 발동
{
    
}
#endregion
