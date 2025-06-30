using UnityEngine;

namespace TK.BehaviourTree
{
    /// <summary>
    /// 자식 하나만을 가지는 부모 노드
    /// </summary>
    public interface IParentOfChild : IParent
    {
        public Node Child { get; set; }
    }

}
