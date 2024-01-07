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
        [SerializeField] GameObject guiContainer;
        [SerializeField] GameObject chosenOptionTemplate;
        [SerializeField] GameObject unchosenOptionTemplate;
        [SerializeField] float optionDistance = 50f;

        private int _currentSelectedOptionIdx = 0;
        private string[] _playerOptions = null;
        private Action<int> _onOptionChosen = null;

        private int _currentSelectedOptionInGui = 0;
        private GameObject[] _playerOptionsGui = null;

        public void DisplayPlayerOptions(string[] options, Action<int> onOptionChosen)
        {
            _currentSelectedOptionIdx = 0;
            _playerOptions = options;
            _onOptionChosen = onOptionChosen;

            guiContainer.SetActive(true);
            AdjustContainerHeight();
            CreateGuiOptions();
        }

        private void CreateGuiOptions()
        {
            if (_playerOptions.Length < 1)
                return;

            _playerOptionsGui = new GameObject[_playerOptions.Length];

            // Stwórz jedną wybraną opcję
            GameObject chosenOption = GameObject.Instantiate(chosenOptionTemplate, guiContainer.transform);
            SetupGuiOption(chosenOption, 0);

            // Stwórz (n - 1) niewybranych opcji
            for(int i = 1; i < _playerOptions.Length; i++)
            {
                GameObject option = GameObject.Instantiate(unchosenOptionTemplate, guiContainer.transform);
                SetupGuiOption(option, i);
            }

            _currentSelectedOptionInGui = 0;

            UpdateOptionsText();
            UpdateOptionsSelection();
        }

        private void SetupGuiOption(GameObject option, int index)
        {
            SetOptionPositionY(option, index * optionDistance);
            option.SetActive(true);

            DialogOptionButton optionButton = option.GetComponent<DialogOptionButton>();
            optionButton.onClick += OnOptionMouseClick;
            optionButton.onHover += OnOptionMouseHover;

            _playerOptionsGui[index] = option;
        }

        private int FindOptionIndex(DialogOptionButton optionButton)
        {
            GameObject clickedGO = optionButton.gameObject;

            for (int i = 0; i < _playerOptionsGui.Length; i++)
            {
                GameObject option = _playerOptionsGui[i];
                if (option == clickedGO)
                {
                    return i;
                }
            }

            return -1;
        }

        private void OnOptionMouseHover(DialogOptionButton optionButton)
        {
            int hoveredOptionIdx = FindOptionIndex(optionButton);

            _currentSelectedOptionIdx = hoveredOptionIdx;
            UpdateOptionsSelection();
        }

        private void OnOptionMouseClick(DialogOptionButton optionButton)
        {
            int clickedOptionIdx = FindOptionIndex(optionButton);

            _currentSelectedOptionIdx = clickedOptionIdx;
            UpdateOptionsSelection();
            OnConfirm();
        }

        private void UpdateOptionsText()
        {
            for(int i = 0; i < _playerOptions.Length; i++)
            {
                GameObject guiOption = _playerOptionsGui[i];
                string textToInsert = _playerOptions[i];

                SetOptionText(guiOption, textToInsert);
            }
        }

        private void SetOptionText(GameObject guiOption, string newText)
        {
            guiOption.GetComponentInChildren<TMPro.TMP_Text>().text = newText;
        }

        private string GetOptionText(GameObject guiOption)
        {
            return guiOption.GetComponentInChildren<TMPro.TMP_Text>().text;
        }

        private void SetOptionPositionY(GameObject guiOption, float positionY)
        {
            RectTransform optionTransform = guiOption.GetComponent<RectTransform>();
            optionTransform.anchoredPosition = new Vector2(optionTransform.anchoredPosition.x, positionY);
        }

        private float GetOptionPositionY(GameObject guiOption)
        {
            return guiOption.GetComponent<RectTransform>().anchoredPosition.y;
        }

        private void AdjustContainerHeight()
        {
            RectTransform containerTransform = guiContainer.GetComponent<RectTransform>();
            containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, _playerOptions.Length * optionDistance);
        }

        private void UpdateOptionsSelection()
        {
            if (_currentSelectedOptionInGui == _currentSelectedOptionIdx)
                return; // Up-to-date

            // Zamień pozycje i teksty ostatnio wybranej opcji z nowowybraną opcją
            // (żeby nie musieć podmieniać stylu)
            int prevSelectedOptionIdx = _currentSelectedOptionInGui;
            GameObject selectedOption = _playerOptionsGui[prevSelectedOptionIdx];
            GameObject unselectedOption = _playerOptionsGui[_currentSelectedOptionIdx];

            string selectedText = GetOptionText(selectedOption);
            string unselectedText = GetOptionText(unselectedOption);
            SetOptionText(selectedOption, unselectedText);
            SetOptionText(unselectedOption, selectedText);

            float selectedPos = GetOptionPositionY(selectedOption);
            float unselectedPos = GetOptionPositionY(unselectedOption);
            SetOptionPositionY(selectedOption, unselectedPos);
            SetOptionPositionY(unselectedOption, selectedPos);

            _playerOptionsGui[_currentSelectedOptionIdx] = selectedOption;
            _playerOptionsGui[prevSelectedOptionIdx] = unselectedOption;

            _currentSelectedOptionInGui = _currentSelectedOptionIdx;
        }

        private void DestroyGuiOptions()
        {
            foreach(GameObject option in _playerOptionsGui)
            {
                GameObject.Destroy(option);
            }

            _playerOptionsGui = null;
        }

        private void Awake()
        {
            guiContainer.SetActive(false);
            chosenOptionTemplate.SetActive(false);
            unchosenOptionTemplate.SetActive(false);
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

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentSelectedOptionIdx++;
                if (_currentSelectedOptionIdx > _playerOptions.Length - 1)
                {
                    _currentSelectedOptionIdx = 0;
                }

                selectedOptionChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
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
                UpdateOptionsSelection();
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
            guiContainer.SetActive(false);
            DestroyGuiOptions();
        }
    }
}
