using System.Collections.Generic;
using UnityEngine;

namespace TK.BehaviourTree
{
    /// <summary>
    /// 자식 여러개를 가지는 부모 노드
    /// </summary>
    public interface IParentOfChildren : IParent
    {
        public List<Node> Children { get; set; }
    }

}
