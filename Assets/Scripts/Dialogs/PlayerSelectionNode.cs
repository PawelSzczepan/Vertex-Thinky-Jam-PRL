using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public abstract class PlayerSelectionNode : DialogNode
    {
        protected List<PlayerResponseNode> _playerOptions = new List<PlayerResponseNode>();
        protected PlayerResponseNode _selectedOption = null;

        public override void AppendToNextNodes(DialogNode newNode)
        {
            PlayerResponseNode playerResponse = newNode as PlayerResponseNode;
            if(playerResponse == null)
            {
                Debug.LogWarning("Trying to add player selection which is not player response");
            }

            _playerOptions.Add(playerResponse);
        }

        public override DialogNode GetNextNode()
        {
            if (_playerOptions.Count > 0 && _selectedOption == null)
                Debug.LogError("None of the available options has been selected");

            return _selectedOption;
        }
    }
}
