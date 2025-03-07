using UnityEngine;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;

    public bool IsDevMode
    {
        get
        {
            return isDevMode;
        }
    }
}
