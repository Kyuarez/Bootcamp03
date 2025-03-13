using UnityEngine;

public static class Define
{
    public static Vector3 Rifle_Pos = new Vector3(0.776f, -0.249f, 0.54f);
    public static Vector3 Rifle_Rotate = new Vector3(-3.946f, 26.312f, 75f);
    public static Vector3 RifleAim_Pos = new Vector3(1.034f, -0.258f, -0.163f);
    public static Vector3 RifleAim_Rotate = new Vector3(14.174f, 101.724f, 78f);

    public const string RES_WEAPONS = "Prefabs/Weapon";
    public const string RES_SO_GUN = "SO/Weapon";
}

#region Enum
public enum GunType
{
    Rifle,
    Sniper,
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
    Damage,
    Idle,
    Die,
}
#endregion
