using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class EditorNpcResponseNode : EditorResponseNode
    {
        public EditorNpcResponseNode()
            : base(NodeType.NpcResponse)
        {
        }

        public override string GetNodeTitle() => "NPC response";

        public override DialogNode ToRuntimeNode()
        {
            return new NpcResponseNode(DialogText);
        }

        protected override bool HasInputPort() => true;
        protected override Port.Capacity GetOutputCapacity() => Port.Capacity.Single;
    }
}
