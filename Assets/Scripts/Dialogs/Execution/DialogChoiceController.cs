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
        [SerializeField] float optionDistances = 50f;

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
            _playerOptionsGui[0] = chosenOption;
            chosenOption.SetActive(true);

            // Stwórz (n - 1) niewybranych opcji
            for(int i = 1; i < _playerOptions.Length; i++)
            {
                GameObject option = GameObject.Instantiate(unchosenOptionTemplate, guiContainer.transform);
                RectTransform optionTransform = option.GetComponent<RectTransform>();
                // Ustaw pozycję
                optionTransform.anchoredPosition = new Vector2(optionTransform.anchoredPosition.x, i * optionDistances);

                _playerOptionsGui[i] = option;
                option.SetActive(true);
            }

            _currentSelectedOptionInGui = 0;

            UpdateOptionsText();
            UpdateOptionsSelection();
        }

        private void UpdateOptionsText()
        {
            for(int i = 0; i < _playerOptions.Length; i++)
            {
                GameObject guiOption = _playerOptionsGui[i];
                string textToInsert = _playerOptions[i];

                UpdateOptionText(guiOption, textToInsert);
            }
        }

        private void UpdateOptionText(GameObject guiOption, string newText)
        {
            guiOption.GetComponentInChildren<TMPro.TMP_Text>().text = newText;
        }

        private void AdjustContainerHeight()
        {
            RectTransform containerTransform = guiContainer.GetComponent<RectTransform>();
            containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, _playerOptions.Length * optionDistances);
        }

        private void UpdateOptionsSelection()
        {
            if (_currentSelectedOptionInGui == _currentSelectedOptionIdx)
                return; // Up-to-date

            // Zamień pozycje i teksty ostatnio wybranej opcji z nowowybraną opcją
            // Zamień pozycje w tabeli
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
            guiContainer.SetActive(false);
        }
    }
}
