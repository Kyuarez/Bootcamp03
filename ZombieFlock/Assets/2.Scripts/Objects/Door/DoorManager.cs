using UnityEngine;

namespace MRG
{
    public enum DoorType
    {
        //앞 뒤로 열리기
        //위 아래로 열리기
        //양 옆으로 열리기
    }

    public class DoorManager : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        private bool isOpen;
        private bool lastOpenedForward;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            
        }

        public bool IsPlayerInFront(Transform player)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.position, toPlayer);
            return dotProduct > 0;
        }

        public bool Open(Transform player)
        {
            if (!isOpen)
            {
                isOpen = true;

                if(IsPlayerInFront(player) == true)
                {
                    anim.SetTrigger("OpenForward");
                    lastOpenedForward = true;
                }
                else
                {
                    anim.SetTrigger("OpenBackward");
                    lastOpenedForward = true;
                }
                return true;
            }

            return false;
        }

        public void CloseForward(Transform player)
        {
            if(isOpen == true)
            {
                isOpen = false;
                anim.SetTrigger("CloseForward");
            }
        }
        public void CloseBackward(Transform player)
        {
            if (isOpen == true)
            {
                isOpen = false;
                anim.SetTrigger("CloseBackward");
            }
        }
    }

}

