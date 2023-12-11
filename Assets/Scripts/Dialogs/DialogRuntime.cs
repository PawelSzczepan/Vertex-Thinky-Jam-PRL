using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogs
{
    public class DialogRuntime
    {
        public void DisplayChosenDialogOption(string dialogOption)
        {
            Debug.Log("[Player] " + dialogOption);
        }

        public void DisplayNpcResponse(string response)
        {
            Debug.Log("[NPC] " + response);
        }
    }
}
