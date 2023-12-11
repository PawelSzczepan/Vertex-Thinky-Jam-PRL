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
            PlayerChoice,
            NpcResponse
        }

        private NodeType _nodeType;

        public NodeType DialogNodeType => _nodeType;


        public EditorDialogNode(NodeType nodeType)
        {
            _nodeType = nodeType;
        }

        public abstract DialogNode ToRuntimeNode();

        protected void AddDefaultOutputPort()
        {
            Port defaultPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)); // TODO: make a key
            defaultPort.portName = "";
            outputContainer.Add(defaultPort);
        }

        protected void AddDefaultInputPort()
        {
            Port defaultPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool)); // TODO: make a key
            defaultPort.portName = "";
            inputContainer.Add(defaultPort);
        }
    }
}
