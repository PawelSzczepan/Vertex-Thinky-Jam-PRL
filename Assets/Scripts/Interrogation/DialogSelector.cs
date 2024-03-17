using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogs;

public class DialogSelector : MonoBehaviour
{
    public static DialogSelector Instance { get; private set; }

    public List<Dialog> dialogs;
    public Dialog defaultDialog;

    public Dialog GetDialog(Character character, QueryItemSO queryItem)
    {
        List<Dialog> fittingDialogs = new List<Dialog>();
        foreach(var d in dialogs)
        {
            if(d.queryItem == queryItem && d.character == character)
            {
                fittingDialogs.Add(d);
            }
        }
        if(fittingDialogs.Count == 0)
        {
            return defaultDialog;
        }

        // TODO: state of the riddle
        Dialog dialog =  fittingDialogs[0];
        fittingDialogs.Clear();
        return dialog;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
