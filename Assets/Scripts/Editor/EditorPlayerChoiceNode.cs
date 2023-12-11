using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public class EditorPlayerChoiceNode : EditorDialogNode
    {
        public EditorPlayerChoiceNode()
            : base(NodeType.PlayerChoice)
        {
            AddDefaultInputPort();
            AddDefaultOutputPort();

            RefreshExpandedState();
            RefreshPorts();
        }

        public override DialogNode ToRuntimeNode()
        {
            return new NpcResponseNode(DialogText);
        }
    }
}
