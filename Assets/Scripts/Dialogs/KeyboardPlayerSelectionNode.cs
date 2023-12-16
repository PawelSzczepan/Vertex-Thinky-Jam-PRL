using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public class KeyboardPlayerSelectionNode : PlayerSelectionNode
    {
        private int _currentSelectedOptionIdx = 0;

        public override ExecuteResult Execute(DialogRuntime runtime)
        {
            if(_playerOptions.Count == 0)
            {
                Debug.LogWarning("Selection node without options");
                return ExecuteResult.Finished;
            }

            if(Input.GetKeyDown(KeyCode.Return))
            { // Zatwierdzenie
                _selectedOption = _playerOptions[_currentSelectedOptionIdx];
                return ExecuteResult.Finished;
            }

            ProcessOptionsNavigation();

            return ExecuteResult.InProgress;
        }

        private void ProcessOptionsNavigation()
        {
            bool selectedOptionChanged = false;

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentSelectedOptionIdx++;
                if(_currentSelectedOptionIdx > _playerOptions.Count - 1)
                {
                    _currentSelectedOptionIdx = 0;
                }

                selectedOptionChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentSelectedOptionIdx--;
                if(_currentSelectedOptionIdx < 0)
                {
                    _currentSelectedOptionIdx = _playerOptions.Count - 1;
                }

                selectedOptionChanged = true;
            }

            if(selectedOptionChanged)
            {
                Debug.Log($"Selected option: {_playerOptions[_currentSelectedOptionIdx].ChoiceText}");
                // TODO: notyfikacja o zmianie opcji
            }
        }
    }
}
