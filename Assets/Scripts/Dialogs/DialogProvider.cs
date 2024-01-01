using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogs
{
    public class DialogProvider : MonoBehaviour
    {
        [SerializeField] TextAsset dialogFile;

        private void Awake()
        {
            DialogGraphView dialogGraphView = new DialogGraphView();
            dialogGraphView.Deserialize(dialogFile.bytes);
        }
    }
}
