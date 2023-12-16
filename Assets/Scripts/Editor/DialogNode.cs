using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogs
{
    public abstract class EditorDialogNode : Node
    {
        public enum NodeType
        {
            PlayerResponse,
            NpcResponse,
            PlayerSelectionNode
        }

        private TextField _dialogTextField;
        private NodeType _nodeType;

        public NodeType DialogNodeType => _nodeType;
        public string DialogText => _dialogTextField.text;


        public EditorDialogNode(NodeType nodeType)
        {
            _nodeType = nodeType;

            _dialogTextField = new TextField();
            _dialogTextField.multiline = true;
            extensionContainer.Add(new Label("Dialog text:"));
            extensionContainer.Add(_dialogTextField);

            RefreshExpandedState();
        }

        public abstract DialogNode ToRuntimeNode();
        public abstract string GetNodeTitle();

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
