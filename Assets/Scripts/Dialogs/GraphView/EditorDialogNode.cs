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
            PlayerResponse,
            NpcResponse,
            KeyboardSelectionNode,
            ThreadStartNode
        }

        private NodeType _nodeType;

        public NodeType DialogNodeType => _nodeType;


        public EditorDialogNode(NodeType nodeType)
        {
            _nodeType = nodeType;

            if(HasInputPort())
            {
                AddInputPort(Port.Capacity.Multi);
            }
            AddOutputPort( GetOutputCapacity() );

            RefreshExpandedState();
            RefreshPorts();
        }

        public abstract DialogNode ToRuntimeNode();
        public abstract string GetNodeTitle();

        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] bytes);

        protected abstract bool HasInputPort(); // Note: should give valid answer before construction
        protected abstract Port.Capacity GetOutputCapacity(); // Note: should give valid answer before construction

        private void AddOutputPort(Port.Capacity capacity)
        {
            Port defaultPort = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(bool));
            defaultPort.portName = "";
            outputContainer.Add(defaultPort);
        }

        private void AddInputPort(Port.Capacity capacity)
        {
            Port defaultPort = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(bool));
            defaultPort.portName = "";
            inputContainer.Add(defaultPort);
        }
    }
}
