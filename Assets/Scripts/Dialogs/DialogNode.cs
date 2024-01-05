using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dialogs
{
    public interface IDialogRuntime
    {
        public event Action onWorkDone;

        public void DisplayPlayerResponse(string dialogOption);

        public void DisplayNpcResponse(string response);
    }

    public abstract class DialogNode
    {
        public enum ExecuteResult
        {
            InProgress,
            Finished,
        }

        public abstract ExecuteResult Execute(IDialogRuntime runtime);

        public abstract DialogNode GetNextNode();
        public abstract void AppendToNextNodes(DialogNode newNode);
    }
}