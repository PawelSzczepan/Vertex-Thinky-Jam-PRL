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
        [SerializeField] float slideSpeed = 10.0f;

        private bool _isDisplayingPlayerOptions = false;
        private float _optionsHeight = 0.0f;

        public void DisplayPlayerResponse(string response, Action onWorkDone)
        {
            Debug.Log("[Player] " + response);
            responsesDisplayer.DisplayDialogText("Player", response, onWorkDone);
        }

        public void DisplayNpcResponse(string response, Action onWorkDone)
        {
            Debug.Log("[NPC] " + response);
            responsesDisplayer.DisplayDialogText("NPC", response, onWorkDone);
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
            _optionsHeight = 0.0f;
        }

        private void Update()
        {
            RectTransform choicesTransform = choiceController.GetComponent<RectTransform>();
            RectTransform responsesTransform = responsesDisplayer.GetComponent<RectTransform>();

            float responsesY = GetY(responsesTransform);
            if (responsesY >= _optionsHeight)
                return; // Sliding finished

            float dY = slideSpeed * Time.deltaTime;
            SetY(choicesTransform, GetY(choicesTransform) + dY);

            SetY(responsesTransform, responsesY + dY);
        }

        private static void SetY(RectTransform rect, float y)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
        }

        private static float GetY(RectTransform rect) => rect.anchoredPosition.y;
    }
}
