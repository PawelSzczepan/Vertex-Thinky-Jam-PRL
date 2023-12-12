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

        public PlayerChoiceData(string textInDialog)
        {
            this.choiceText = textInDialog;
            this.textInDialog = textInDialog;
        }
    }

    public class PlayerChoiceNode : NoChoiceDialogNode
    {
        private PlayerChoiceData _data;

        public PlayerChoiceNode(PlayerChoiceData data)
        {
            _data = data;
        }

        public PlayerChoiceNode(string dialogText)
        {
            _data = new PlayerChoiceData(dialogText);
        }

        public override ExecuteResult Execute(DialogRuntime runtime)
        {
            runtime.DisplayChosenDialogOption(_data.textInDialog);
            return ExecuteResult.Finished;
        }
    }
}
