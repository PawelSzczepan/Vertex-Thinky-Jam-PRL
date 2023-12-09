using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public abstract class DialogNode : Node
    {
        public enum NodeType
        {
            ThreadStart,
            PlayerChoice,
            NpcResponse
        }

        private NodeType _nodeType;

        public DialogNode(NodeType nodeType)
        {
            _nodeType = nodeType;
        }
    }
}
