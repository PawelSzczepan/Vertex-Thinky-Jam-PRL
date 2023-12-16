using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public class EditorPlayerResponseNode : EditorDialogNode
    {
        public EditorPlayerResponseNode()
            : base(NodeType.PlayerResponse)
        {
            AddDefaultInputPort();
            AddDefaultOutputPort();

            RefreshExpandedState();
            RefreshPorts();
        }

        public override string GetNodeTitle() => "Player choice";

        public override DialogNode ToRuntimeNode()
        {
            return new NpcResponseNode(DialogText);
        }
    }
}
