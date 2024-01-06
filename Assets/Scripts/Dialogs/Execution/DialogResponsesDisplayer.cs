using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Dialogs
{
    public class DialogResponsesDisplayer : MonoBehaviour
    {
        [SerializeField] TMP_Text newestDialogText;
        [SerializeField] TMP_Text dialogHistory;
        [SerializeField] int historySize = 2;

        private Queue<string> historyTexts = new Queue<string>();

        public void DisplayDialogText(string text, Action onWorkDone)
        {
            AddToHistory(newestDialogText.text);
            // TODO: powolne wypisywanie tekstu
            newestDialogText.text = text;
            StartCoroutine(CooldownDialogText(onWorkDone)); // Testowe spowolnienie wyświetlania następnego tekstu
        }

        private IEnumerator CooldownDialogText(Action onFinished)
        {
            yield return new WaitForSeconds(1f);
            onFinished?.Invoke();
        }

        private void AddToHistory(string text)
        {
            while (historyTexts.Count >= historySize)
            {
                historyTexts.Dequeue();
            }

            historyTexts.Enqueue(text);

            DisplayDialogHistory();
        }

        private void DisplayDialogHistory()
        {
            string resultText = "";

            for (int i = 0; i < historyTexts.Count; i++)
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
