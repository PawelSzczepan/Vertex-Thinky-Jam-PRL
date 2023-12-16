using System;
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
            : base(NodeType.PlayerSelectionNode)
        {

        }

        public override string GetNodeTitle() => "Keyboard selection";

        public override DialogNode ToRuntimeNode()
        {
            return new KeyboardSelectionNode();
        }

        protected override bool HasInputPort() => true;

        protected override Port.Capacity GetOutputCapacity() => Port.Capacity.Multi;
    }
}
