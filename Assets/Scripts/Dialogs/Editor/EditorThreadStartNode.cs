using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class ThreadStartNode : EditorResponseNode
    {
        public ThreadStartNode()
            : base(NodeType.ThreadStartNode)
        {

        }

        public override string GetNodeTitle() => "Thread start";

        public override DialogNode ToRuntimeNode()
        {
            return new PlayerResponseNode(new PlayerChoiceData
            {
                choiceText = DialogText
            });
        }


        protected override bool HasInputPort() => false;
        protected override Port.Capacity GetOutputCapacity() => Port.Capacity.Single;
    }
}
