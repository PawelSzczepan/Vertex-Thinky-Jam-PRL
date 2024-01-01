using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public class DialogExecutor : MonoBehaviour
    {
        public static DialogExecutor Instance { get; private set; }

        private DialogSession _currentDialogSession;

        public void StartDialog(DialogNode dialogStart)
        {
            _currentDialogSession = new DialogSession(dialogStart);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            if (_currentDialogSession == null)
                return;

            bool is_running = _currentDialogSession.Execute();
            if(!is_running)
            {
                Debug.Log("Dialog end");
                _currentDialogSession = null;
            }
        }

        private class DialogSession
        {
            private DialogNode _currentNode;
            private DialogRuntime runtime = new DialogRuntime();

            public DialogSession(DialogNode startNode)
            {
                _currentNode = startNode;
            }

            public bool Execute()
            {
                if (_currentNode == null)
                    return false; // Koniec dialogu

                DialogNode.ExecuteResult result = _currentNode.Execute(runtime);

                if (result == DialogNode.ExecuteResult.Finished)
                {
                    _currentNode = _currentNode.GetNextNode();
                }
                // else - Jeśli node dalej się wykonuje, to zostajemy przy tym samym
                return true;
            }
        }
    }
}
