using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public class DialogChoiceController : MonoBehaviour
    {
        private int _currentSelectedOptionIdx = 0;
        private string[] _playerOptions = null;
        private Action<int> _onOptionChosen = null;

        public void DisplayPlayerOptions(string[] options, Action<int> onOptionChosen)
        {
            _currentSelectedOptionIdx = 0;
            _playerOptions = options;
            _onOptionChosen = onOptionChosen;
        }

        private void Update()
        {
            if (_playerOptions == null)
                return;

            ProcessOptionsNavigation();
        }

        private void ProcessOptionsNavigation()
        {
            bool selectedOptionChanged = false;

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentSelectedOptionIdx++;
                if (_currentSelectedOptionIdx > _playerOptions.Length - 1)
                {
                    _currentSelectedOptionIdx = 0;
                }

                selectedOptionChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentSelectedOptionIdx--;
                if (_currentSelectedOptionIdx < 0)
                {
                    _currentSelectedOptionIdx = _playerOptions.Length - 1;
                }

                selectedOptionChanged = true;
            }

            if (selectedOptionChanged)
            {
                Debug.Log($"Selected option: {_playerOptions[_currentSelectedOptionIdx]}");
            }


            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnConfirm();
            }
        }

        private void OnConfirm()
        {
            _onOptionChosen?.Invoke(_currentSelectedOptionIdx);

            _playerOptions = null;
        }
    }
}
