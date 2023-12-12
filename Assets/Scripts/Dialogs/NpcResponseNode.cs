using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public class NpcResponseNode : NoChoiceDialogNode
    {
        private string _responseText;

        public NpcResponseNode(string text)
        {
            _responseText = text;
        }

        public override ExecuteResult Execute(DialogRuntime runtime)
        {
            runtime.DisplayNpcResponse(_responseText);
            return ExecuteResult.Finished;
        }
    }
}
