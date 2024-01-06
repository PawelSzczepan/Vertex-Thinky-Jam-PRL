using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public class PlayerSelectionNode : DialogNode
    {
        private enum State { None, ChoicesDisplayed, OptionChosed }
        private State _state = State.None;

        private List<PlayerResponseNode> _playerOptions = new List<PlayerResponseNode>();
        private PlayerResponseNode _selectedOption = null;

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

        public override ExecuteResult Execute(IDialogRuntime runtime)
        {
            if (_playerOptions.Count == 0)
            {
                Debug.LogWarning("Selection node without options");
                return ExecuteResult.Finished;
            }

            switch (_state)
            {
                case State.None:
                    runtime.DisplayPlayerOptions(_playerOptions.Select(node => node.ChoiceText).ToArray(), OnOptionChosed);
                    _state = State.ChoicesDisplayed;
                    return ExecuteResult.InProgress;

                case State.ChoicesDisplayed:
                    return ExecuteResult.InProgress;

                case State.OptionChosed:
                    _state = State.None;
                    return ExecuteResult.Finished;
            }

            throw new NotImplementedException();
        }

        private void OnOptionChosed(int optionIndex)
        {
            _selectedOption = _playerOptions[optionIndex];
            _state = State.OptionChosed;
        }
    }
}
