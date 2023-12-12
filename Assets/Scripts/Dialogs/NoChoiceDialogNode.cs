using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public abstract class NoChoiceDialogNode : DialogNode
    {
        private DialogNode _nextNode = null;

        public override DialogNode GetNextNode()
        {
            return _nextNode;
        }

        public override void AppendToNextNodes(DialogNode newNode)
        {
            if (_nextNode != null)
                Debug.LogError("Reassigning next node");

            _nextNode = newNode;
        }
    }
}
