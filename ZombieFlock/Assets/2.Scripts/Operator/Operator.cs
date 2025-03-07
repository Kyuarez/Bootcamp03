using UnityEngine;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;

    private PlayerManager ingamePlayer;
    public bool IsDevMode
    {
        get
        {
            return isDevMode;
        }
    }

    public PlayerManager PlayerManager
    {
        get 
        {
            if (ingamePlayer == null) 
            {
                ingamePlayer = Object.FindFirstObjectByType<PlayerManager>();
            }
            
            return ingamePlayer; 
        }
    }
}
