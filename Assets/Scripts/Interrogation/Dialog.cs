using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogs;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 2)]
public class Dialog : ScriptableObject
{
    public QueryItemSO queryItem;
    public Character character;
    public TextAsset dialogFile;
}
