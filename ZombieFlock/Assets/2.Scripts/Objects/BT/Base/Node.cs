using UnityEngine;

namespace TK.BehaviourTree
{
    public abstract class Node : MonoBehaviour
    {
        protected BT tree;
        //TODO : Blackboard

        public Node(BT tree)
        {
            this.tree = tree;
        }

        public abstract Result Invoke();
        public virtual void OnDrawGizmos() { }
    }
}

