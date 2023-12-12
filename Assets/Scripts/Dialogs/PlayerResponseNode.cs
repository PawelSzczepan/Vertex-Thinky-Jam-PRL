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

    public class PlayerResponseNode : NoChoiceDialogNode
    {
        private PlayerChoiceData _data;

        public PlayerResponseNode(PlayerChoiceData data)
        {
            _data = data;
        }

        public PlayerResponseNode(string dialogText)
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
