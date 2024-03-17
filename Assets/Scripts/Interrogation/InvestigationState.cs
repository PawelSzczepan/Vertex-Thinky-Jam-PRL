using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationState : MonoBehaviour
{
    public static InvestigationState Instance { get; private set; }

    public Character selectedCharacter { get; private set; }
    public QueryItemSO selectedQueryItem { get; private set; }

    private void OnCharacterSelected(Character character)
    {
        selectedCharacter = character;
        AssemblyDialog(selectedCharacter, selectedQueryItem);
    }

    private void OnQueryItemSelected(QueryItemSO item)
    {
        selectedQueryItem = item;
    }

    private void AssemblyDialog(Character character, QueryItemSO item)
    {
        Dialog dialog = DialogSelector.Instance.GetDialog(character, item);
        InvestigationEvents.InvokeRunDialog(dialog);
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
