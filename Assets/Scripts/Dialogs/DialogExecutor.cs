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
        private DialogSession _currentDialogSession;

        private DialogNode CreateTestDialogGraph()
        {
            DialogNode threadStart = new PlayerChoiceNode("Dzień dobry, Pani!");

            DialogNode npcResponse = new NpcResponseNode("Dzień dobry, Panu!");
            threadStart.AppendToNextNodes(npcResponse);

            return threadStart;
        }

        private void Awake()
        {
            _currentDialogSession = new DialogSession(CreateTestDialogGraph());
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
