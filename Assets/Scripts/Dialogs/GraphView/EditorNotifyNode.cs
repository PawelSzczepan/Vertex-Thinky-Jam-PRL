using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Text;
using UnityEngine.UIElements;
using System;

namespace Dialogs
{
    public class EditorNotifyNode : EditorDialogNode
    {
        public string Id => _textField.value;
        private TextField _textField;

        public EditorNotifyNode()
            : base(NodeType.NotifyNode)
        {
            _textField = new TextField();
            extensionContainer.Add(new Label("Notification id:"));
            extensionContainer.Add(_textField);

            RefreshExpandedState();
        }

        public override string GetNodeTitle() => "Notify";


        public override DialogNode ToRuntimeNode()
        {
            return new NotifyNode(_textField.value);
        }

        public override byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes(_textField.value);
        }

        public override void Deserialize(byte[] bytes)
        {
            _textField.value = Encoding.UTF8.GetString(bytes);
        }

        protected override bool HasInputPort() => true;

        protected override Port.Capacity GetOutputCapacity() => Port.Capacity.Single;
    }
}