﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class EditorKeyboardSelectionNode : EditorDialogNode
    {
        public EditorKeyboardSelectionNode()
            : base(NodeType.KeyboardSelectionNode)
        {

        }

        public override string GetNodeTitle() => "Keyboard selection";

        public override DialogNode ToRuntimeNode()
        {
            return new KeyboardSelectionNode();
        }

        public override byte[] Serialize()
        {
            return new byte[0]; // Nothing to save
        }

        public override void Deserialize(byte[] bytes)
        {
            return;
        }

        protected override bool HasInputPort() => true;

        protected override Port.Capacity GetOutputCapacity() => Port.Capacity.Multi;
    }
}