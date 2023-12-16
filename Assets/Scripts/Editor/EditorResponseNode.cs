using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogs
{
    public abstract class EditorResponseNode : EditorDialogNode
    {
        private TextField _dialogTextField;
        public string DialogText => _dialogTextField.text;

        public EditorResponseNode(NodeType nodeType)
            : base(nodeType)
        {
            _dialogTextField = new TextField();
            _dialogTextField.multiline = true;
            extensionContainer.Add(new Label("Dialog text:"));
            extensionContainer.Add(_dialogTextField);

            RefreshExpandedState();
        }
    }
}
