using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public class NpcResponseNode : ResponseNode
    {
        private string _responseText;

        public NpcResponseNode(string text)
        {
            _responseText = text;
        }

        protected override void RequestDisplayingResponse(IDialogRuntime runtime)
        {
            runtime.DisplayNpcResponse(_responseText);
        }
    }
}
