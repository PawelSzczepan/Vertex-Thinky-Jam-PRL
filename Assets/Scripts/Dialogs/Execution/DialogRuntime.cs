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
            choiceController.DisplayPlayerOptions(options, onOptionChosen);
        }
    }
}
