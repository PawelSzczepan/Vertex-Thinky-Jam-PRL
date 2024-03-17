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
        [SerializeField] float timeBetweenLetters = 0.1f;
        [SerializeField] float timeAfterFinish = 1f;

        private Queue<string> _historyTexts = new Queue<string>();
        private string _newestActorLabel = "";
        private bool _isTextConstructionInProgress = false;
        private SlowTextConstructor _textUpdater;
        private Action _onWorkDone = null;

        public void DisplayDialogText(string actorLabel, string text, Action onWorkDone)
        {
            AddToHistory(newestDialogText.text);

            // Display the actor label now, add the rest progressively
            _newestActorLabel = $"{actorLabel}: ";
            newestDialogText.text = _newestActorLabel;
            _isTextConstructionInProgress = true;
            _onWorkDone = onWorkDone;

            _textUpdater.RequestText(text);
        }

        public void ClearHistory()
        {
            _historyTexts.Clear();
            newestDialogText.text = "";
        }

        private void OnTextFinished()
        {
            _isTextConstructionInProgress = false;
            StartCoroutine(InvokeWorkDoneAfterTime(timeAfterFinish));
        }

        private IEnumerator InvokeWorkDoneAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            _onWorkDone?.Invoke();
        }

        private void AddToHistory(string text)
        {
            while (_historyTexts.Count >= historySize)
            {
                _historyTexts.Dequeue();
            }

            _historyTexts.Enqueue(text);

            DisplayDialogHistory();
        }

        private void DisplayDialogHistory()
        {
            string resultText = "";

            for (int i = 0; i < _historyTexts.Count; i++)
            {
                string historyEntry = _historyTexts.Dequeue();
                resultText += historyEntry + '\n';
                _historyTexts.Enqueue(historyEntry);
            }

            dialogHistory.text = resultText;
        }

        private void Update()
        {
            if (!_isTextConstructionInProgress)
                return;

            _textUpdater.Update();
            newestDialogText.text = _newestActorLabel + _textUpdater.CurrentText;
        }

        private void Awake()
        {
            dialogHistory.text = "";
            newestDialogText.text = "";

            _textUpdater = new SlowTextConstructor(timeBetweenLetters);
            _textUpdater.onTextFinished += OnTextFinished;
        }

        private void OnEnable()
        {
            ClearHistory();
        }

        private void OnDisable()
        {
            ClearHistory();
        }

        private class SlowTextConstructor
        {
            public event Action onTextFinished;

            private float _timePerLetter;
            private float _timeSinceLastLetter;
            private string _requestedText = "";

            public SlowTextConstructor(float timePerLetter)
            {
                _timePerLetter = timePerLetter;
            }

            public string CurrentText { get; private set; } = "";

            public void RequestText(string text)
            {
                _timeSinceLastLetter = 0;
                _requestedText = text;
                CurrentText = "";
            }

            public void Update()
            {
                if(CurrentText.Length == _requestedText.Length)
                {
                    onTextFinished?.Invoke();
                    return;
                }

                while(_timeSinceLastLetter >= _timePerLetter)
                {
                    AddLetter();
                    _timeSinceLastLetter -= _timePerLetter;

                    if (CurrentText.Length == _requestedText.Length)
                    {
                        onTextFinished?.Invoke();
                        return;
                    }
                }

                _timeSinceLastLetter += Time.deltaTime;
            }

            private void AddLetter()
            {
                char nextLetter = _requestedText[CurrentText.Length];
                CurrentText += nextLetter;
            }
        }
    }
}
