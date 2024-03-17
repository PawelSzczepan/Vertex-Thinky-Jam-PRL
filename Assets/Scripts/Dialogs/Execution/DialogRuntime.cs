using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public class DialogRuntime : MonoBehaviour, IDialogRuntime
    {
        [SerializeField] DialogResponsesDisplayer responsesDisplayer;
        [SerializeField] DialogChoiceController choiceController;
        [SerializeField] float slideTime = 1.0f;
        [SerializeField] string playerName = "Milicjant";

        private bool _isDisplayingPlayerOptions = false;
        private float _optionsHeight = 0.0f;

        public void DisplayPlayerResponse(string response, Action onWorkDone)
        {
            Debug.Log(playerName + response);
            responsesDisplayer.DisplayDialogText(playerName, response, onWorkDone);
        }

        public void DisplayNpcResponse(string response, Action onWorkDone)
        {
            Debug.Log(InvestigationState.Instance.selectedCharacter.nickname + ":" + response);
            responsesDisplayer.DisplayDialogText(InvestigationState.Instance.selectedCharacter.nickname, response, onWorkDone);
        }

        public void DisplayPlayerOptions(string[] options, Action<int> onOptionChosen)
        {
            if(_isDisplayingPlayerOptions)
            {
                Debug.LogError("Choosing previous player options hasn't finished");
            }

            _isDisplayingPlayerOptions = true;

            choiceController.DisplayPlayerOptions(options,
                onOptionChosen: (int optionIdx) =>
            {
                OnPlayerOptionChosen();
                onOptionChosen?.Invoke(optionIdx);
            },
            out _optionsHeight);

            SetY(choiceController.GetComponent<RectTransform>(), -_optionsHeight); // Hide the options just beneath the screen
        }


        private void OnPlayerOptionChosen()
        {
            _isDisplayingPlayerOptions = false;
        }

        private void Update()
        {
            if(_isDisplayingPlayerOptions)
            {
                Update_SlideOptionsIn();
            }
            else
            {
                Update_Reset();
            }
        }

        private void Update_SlideOptionsIn()
        {
            RectTransform choicesTransform = choiceController.GetComponent<RectTransform>();
            RectTransform responsesTransform = responsesDisplayer.GetComponent<RectTransform>();

            float responsesY = GetY(responsesTransform);
            if (responsesY >= _optionsHeight)
                return; // Sliding finished

            float dY = _optionsHeight / slideTime * Time.deltaTime;
            SetY(choicesTransform, GetY(choicesTransform) + dY);

            SetY(responsesTransform, responsesY + dY);
        }

        private void Update_Reset()
        {
            RectTransform responsesTransform = responsesDisplayer.GetComponent<RectTransform>();

            float responsesY = GetY(responsesTransform);
            if (responsesY <= 0)
                return; // Resetting finished

            float dY = _optionsHeight / slideTime * Time.deltaTime;
            SetY(responsesTransform, responsesY - dY);
        }

        private static void SetY(RectTransform rect, float y)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
        }

        private static float GetY(RectTransform rect) => rect.anchoredPosition.y;
    }
}
