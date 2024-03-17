using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterTile : MonoBehaviour
{
    public TMP_Text characterName;
    public Character character;

    public void Initialize(Character character)
    {
        this.character = character;
        characterName.text = character.nickname;
    }

    public void SelectCharacter()
    {
        if(character!=null)
        {
            InvestigationEvents.InvokeCharacterChosen(character);
        }
    }
}
