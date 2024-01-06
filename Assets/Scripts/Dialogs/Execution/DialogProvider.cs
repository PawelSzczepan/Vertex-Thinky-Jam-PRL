using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogs
{
    public class DialogProvider : MonoBehaviour
    {
        [SerializeField] TextAsset dialogFile;
        DialogNode _dialog;

        private void Awake()
        {
            DialogGraphView dialogGraphView = new DialogGraphView();
            dialogGraphView.Deserialize(dialogFile.bytes);
            _dialog = dialogGraphView.GetRuntimeGraph();
        }

        private void Start()
        {
            DialogExecutor.Instance.StartDialog(_dialog);
        }
    }
}
