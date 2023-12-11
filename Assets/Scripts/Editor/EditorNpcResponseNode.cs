using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public class EditorNpcResponseNode : EditorDialogNode
    {
        public EditorNpcResponseNode()
            : base(NodeType.NpcResponse)
        {
            AddDefaultInputPort();
            AddDefaultOutputPort();

            RefreshExpandedState();
            RefreshPorts();
        }

        public override DialogNode ToRuntimeNode()
        {
            return new NpcResponseNode("Example response");
        }
    }
}
