using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public abstract class EditorDialogNode : Node
    {
        public enum NodeType
        {
            ThreadStart,
            PlayerChoice,
            NpcResponse
        }

        private NodeType _nodeType;

        public EditorDialogNode(NodeType nodeType)
        {
            _nodeType = nodeType;
        }
    }
}
