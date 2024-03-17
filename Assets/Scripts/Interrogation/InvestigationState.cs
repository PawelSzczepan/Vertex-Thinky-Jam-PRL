using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationState : MonoBehaviour
{
    private Character selectedCharacter;
    private QueryItem selectedQueryItem;

    private void OnCharacterSelected(Character character)
    {
        selectedCharacter = character;
    }

    private void OnQueryItemSelected(QueryItem item)
    {
        selectedQueryItem = item;
    }

    private void OnEnable()
    {
        InvestigationEvents.CharacterChosen += OnCharacterSelected;
        InvestigationEvents.QueryItemSelected += OnQueryItemSelected;
    }

    private void OnDisable()
    {
        InvestigationEvents.CharacterChosen -= OnCharacterSelected;
        InvestigationEvents.QueryItemSelected -= OnQueryItemSelected;
    }
}
