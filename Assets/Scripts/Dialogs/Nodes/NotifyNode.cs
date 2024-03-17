using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogs
{
    public class NotifyNode : NoChoiceDialogNode
    {
        private string _id;

        public NotifyNode(string id)
        {
            _id = id;
        }
        public override ExecuteResult Execute(IDialogRuntime runtime)
        {
            Debug.Log($"executed node {_id}");
            return ExecuteResult.Finished;
        }
    }
}
