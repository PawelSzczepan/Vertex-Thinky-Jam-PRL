using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class InvestigationEvents
{
    public static event Action<QueryItem> QueryItemSelected;
    public static void InvokeQueryItemSelected(QueryItem queryItem)
    {
        QueryItemSelected?.Invoke(queryItem);
    }

    public static event Action<Character> CharacterChosen;
    public static void InvokeCharacterChosen(Character character)
    {
        CharacterChosen?.Invoke(character);
    }
}
