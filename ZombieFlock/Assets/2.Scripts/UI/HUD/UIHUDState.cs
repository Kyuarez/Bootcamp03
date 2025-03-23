using UnityEngine;

public class UIHUDState : MonoBehaviour
{
    public void SetActiveState(bool active)
    {
        if(gameObject.activeSelf == !active)
        {
            gameObject.SetActive(active);
        }
    }
}
