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
            : base(NodeType.PlayerChoice)
        {
            // Początek wątku nie ma wejść
            // I ma tylko jedno wyjście - pasujące tylko do NpcResponseNode
            AddDefaultOutputPort();

            RefreshExpandedState();
            RefreshPorts();
        }

        public override DialogNode ToRuntimeNode()
        {
            return new PlayerChoiceNode(new PlayerChoiceData
            {
                choiceText = "Thread Start. Hello there!"
            });
        }
    }
}
