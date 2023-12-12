using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class ThreadStartNode : EditorDialogNode
    {
        public ThreadStartNode()
            : base(NodeType.PlayerResponse)
        {
            // Początek wątku nie ma wejść
            // I ma tylko jedno wyjście - pasujące tylko do NpcResponseNode
            AddDefaultOutputPort();

            RefreshExpandedState();
            RefreshPorts();
        }

        public override DialogNode ToRuntimeNode()
        {
            return new PlayerResponseNode(new PlayerChoiceData
            {
                choiceText = DialogText
            });
        }
    }
}
