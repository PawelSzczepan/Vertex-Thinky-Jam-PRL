using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterTile : MonoBehaviour
{
    public TMP_Text characterName;
    public Character character;
    public Image portrait;

    public void Initialize(Character character)
    {
        this.character = character;
        characterName.text = character.nickname;
        portrait.sprite = character.portrait;
    }

    public void SelectCharacter()
    {
        if(character!=null)
        {
            InvestigationEvents.InvokeCharacterChosen(character);
        }
    }
}
