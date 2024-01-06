using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public interface IDialogRuntime
    {
        public void DisplayPlayerResponse(string dialogOption, Action onWorkDone);
        public void DisplayNpcResponse(string response, Action onWorkDone);

        public void DisplayPlayerOptions(string[] options, Action<int> onOptionChosen);
    }
}
