using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class ThreadStartNode : DialogNode
    {
        public ThreadStartNode()
            : base(nodeType: NodeType.ThreadStart)
        {
            // Początek wątku nie ma wejść
            // I ma tylko jedno wyjście - pasujące tylko do NpcResponseNode
            AddDefaultOutputPort();

            RefreshExpandedState();
            RefreshPorts();
        }

        private void AddDefaultOutputPort()
        {
            Port defaultPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)); // TODO: make a key
            defaultPort.portName = "";
            outputContainer.Add(defaultPort);
        }
    }
}
