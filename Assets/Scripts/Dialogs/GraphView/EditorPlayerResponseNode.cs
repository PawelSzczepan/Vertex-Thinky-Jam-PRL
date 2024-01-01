using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class EditorPlayerResponseNode : EditorResponseNode
    {
        public EditorPlayerResponseNode()
            : base(NodeType.PlayerResponse)
        {
        }

        public override string GetNodeTitle() => "Player choice";

        public override DialogNode ToRuntimeNode()
        {
            return new PlayerResponseNode(DialogText);
        }

        protected override bool HasInputPort() => true;
        protected override Port.Capacity GetOutputCapacity() => Port.Capacity.Single;
    }
}
