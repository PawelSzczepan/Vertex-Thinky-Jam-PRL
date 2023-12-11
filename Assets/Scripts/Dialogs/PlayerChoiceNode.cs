using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
    public struct PlayerChoiceData
    {
        public string choiceText;
        public string textInDialog;
    }

    public class PlayerChoiceNode : DialogNode
    {
        private PlayerChoiceData _data;

        public PlayerChoiceNode(PlayerChoiceData data)
        {
            _data = data;
        }

        public override ExecuteResult Execute(DialogRuntime runtime)
        {
            runtime.DisplayChosenDialogOption(_data.textInDialog);
            return ExecuteResult.Finished;
        }
    }
}
