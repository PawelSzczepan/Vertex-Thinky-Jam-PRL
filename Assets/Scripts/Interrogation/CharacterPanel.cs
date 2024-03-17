using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private List<Character> characters;
    [SerializeField] private GameObject characterTilePrefab;
    [SerializeField] private float radius;
    [SerializeField] private GameObject hideRoot;
    [SerializeField] private GameObject characterTilesParent;
    [SerializeField] private TMP_Text title;

    private List<GameObject> characterTiles;
    private RectTransform rectTransform;
    

    private void Show(List<Character> characters)
    {
        characterTiles = new List<GameObject>();
        foreach (Character character in characters)
        {
            GameObject newTile = Instantiate(characterTilePrefab, characterTilesParent.transform);
            CharacterTile tileScript = newTile.GetComponent<CharacterTile>();
            Debug.Assert(tileScript != null);
            tileScript.Initialize(character);
            characterTiles.Add(newTile);
        }
        PositionTiles();
        hideRoot.SetActive(true);
    }

    private void Hide()
    {
        if (characterTiles != null)
        {
            foreach (var tile in characterTiles)
            {
                Destroy(tile);
            }
            characterTiles.Clear();
        }
        hideRoot.SetActive(false);
    }

    private void PositionTiles()
    {
        float angleStep = 360f / (characterTiles.Count) * Mathf.Deg2Rad;
        for (int i = 0; i < characterTiles.Count; i++)
        {
            RectTransform tileRectTransform = characterTiles[i].GetComponent<RectTransform>();
            Vector2 offset = new Vector2(Mathf.Cos(i*angleStep - Mathf.PI/2), Mathf.Sin(i* angleStep - Mathf.PI / 2)) * radius;
            tileRectTransform.position = offset + new Vector2(Screen.width/2, Screen.height/2);
        }
    }

    private void SetTitle(string val)
    {
        title.text = val;
    }

    private void OnQueryItemSelected(QueryItemSO queryItem)
    {
        Show(characters);
        SetTitle(queryItem.name);
    }

    private void OnCharacterChosen(Character character)
    {
        Hide();
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void OnEnable()
    {
        InvestigationEvents.QueryItemSelected += OnQueryItemSelected;    
        InvestigationEvents.CharacterChosen += OnCharacterChosen;
        InvestigationEvents.Unfocused += Hide;
    }

    private void OnDisable()
    {
        InvestigationEvents.QueryItemSelected -= OnQueryItemSelected;
        InvestigationEvents.CharacterChosen -= OnCharacterChosen;
        InvestigationEvents.Unfocused -= Hide;
    }
}
