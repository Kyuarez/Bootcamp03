using UnityEngine;

namespace TK.BehaviourTree
{
    public interface IParent 
    {
        /// <summary>
        /// 노드의 자식 붙이는 메소드
        /// </summary>
        void AttachChild(Node node);
    }

}
