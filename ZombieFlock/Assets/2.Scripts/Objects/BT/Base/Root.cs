using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK.BehaviourTree
{
    public class Root : Node, IParentOfChild
    {
        public Root(BT tree) : base(tree) { }
        
        public Node Child { get; set; }

        public void AttachChild(Node node)
        {
            Child = node;
        }

        public override Result Invoke()
        {
            return Child.Invoke();
        }
    }
}
 
