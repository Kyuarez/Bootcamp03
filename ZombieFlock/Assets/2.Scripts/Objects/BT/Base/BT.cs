using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TK.BehaviourTree
{
    /// <summary>
    /// Behaviour Tree
    /// </summary>
    public class BT : MonoBehaviour
    {
        //TODO Blackboard
        public Root root { get; set; }

        private bool _isRunning;
        private Stack<Node> _nodeStack;

        private void Awake()
        {
            root = new Root(this);
            _nodeStack = new Stack<Node>();
        }

        private void Update()
        {
            if (_isRunning) 
            {
                return;
            }
            
            _isRunning = true;
            StartCoroutine(CoTick());
        }

        /// <summary>
        /// Route Behaviour Tree : stack DFS 
        /// </summary>
        private IEnumerator CoTick()
        {
            _nodeStack.Push(root);

            while(_nodeStack.Count > 0)
            {
                Node search = _nodeStack.Pop();
                Result result = search.Invoke();

                if(result == Result.Running)
                {
                    _nodeStack.Push(search);
                    yield return null;  
                }
            }

            _isRunning = false;
        }

        private void OnDrawGizmos()
        {
            if (_nodeStack == null)
            {
                return;
            }

            if (_nodeStack.Count > 0)
            {
                _nodeStack.Peek().OnDrawGizmos();
            }
        }
    }
}
