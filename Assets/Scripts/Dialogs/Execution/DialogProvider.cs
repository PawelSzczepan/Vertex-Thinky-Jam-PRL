using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogs
{
    public class DialogProvider : MonoBehaviour
    {
        private DialogNode _dialog;

        private void RunDialog(Dialog dialog)
        {
            DialogGraphView dialogGraphView = new DialogGraphView();
            dialogGraphView.Deserialize(dialog.dialogFile.bytes);
            _dialog = dialogGraphView.GetRuntimeGraph();
            DialogExecutor.Instance.StartDialog(_dialog);
        }


        private void OnEnable()
        {
            InvestigationEvents.RunDialog += RunDialog;
        }

        private void OnDisable()
        {
            InvestigationEvents.RunDialog -= RunDialog;
        }
    }
}
