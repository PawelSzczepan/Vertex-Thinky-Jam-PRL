using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class InvestigationEvents
{
    public static event Action<QueryItemSO> QueryItemSelected;
    public static void InvokeQueryItemSelected(QueryItemSO queryItem)
    {
        QueryItemSelected?.Invoke(queryItem);
    }

    public static event Action<Character> CharacterChosen;
    public static void InvokeCharacterChosen(Character character)
    {
        CharacterChosen?.Invoke(character);
    }

    public static event Action Unfocused;
    public static void InvokeUnfocused()
    {
        Unfocused?.Invoke();
    }

    public static event Action<Dialog> RunDialog;
    public static void InvokeRunDialog(Dialog line)
    {
        RunDialog?.Invoke(line);
    }

    public static event Action DialogFinished;
    public static void InvokeDialogFinished()
    {
        DialogFinished?.Invoke();
    }
}
