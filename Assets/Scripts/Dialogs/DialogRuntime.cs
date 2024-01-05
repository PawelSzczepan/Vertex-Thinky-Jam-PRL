using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Dialogs
{
    public class DialogRuntime : MonoBehaviour, IDialogRuntime
    {
        [SerializeField] TMP_Text newestDialogText;
        [SerializeField] TMP_Text dialogHistory;
        [SerializeField] int historySize = 2;

        public event Action onWorkDone;

        private Queue<string> historyTexts = new Queue<string>();

        public void DisplayPlayerResponse(string response)
        {
            Debug.Log("[Player] " + response);
            DisplayDialogText("[Player] " + response);
        }

        public void DisplayNpcResponse(string response)
        {
            Debug.Log("[NPC] " + response);
            DisplayDialogText("[NPC] " + response);
        }

        private void DisplayDialogText(string text)
        {
            AddToHistory(newestDialogText.text);
            // TODO: powolne wypisywanie tekstu
            newestDialogText.text = text;
            StartCoroutine(CooldownDialogText()); // Testowe spowolnienie wyświetlania następnego tekstu
        }

        private IEnumerator CooldownDialogText()
        {
            yield return new WaitForSeconds(1f);
            onWorkDone?.Invoke();
        }

        private void AddToHistory(string text)
        {
            while(historyTexts.Count >= historySize)
            {
                historyTexts.Dequeue();
            }

            historyTexts.Enqueue(text);

            DisplayDialogHistory();
        }

        private void DisplayDialogHistory()
        {
            string resultText = "";

            for(int i = 0; i < historyTexts.Count; i++)
            {
                string historyEntry = historyTexts.Dequeue();
                resultText += historyEntry + '\n';
                historyTexts.Enqueue(historyEntry);
            }

            dialogHistory.text = resultText;
        }

        private void Awake()
        {
            dialogHistory.text = "";
            newestDialogText.text = "";
        }
    }
}
