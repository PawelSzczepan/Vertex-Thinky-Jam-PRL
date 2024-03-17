using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryItem : MonoBehaviour
{
    [SerializeField] private QueryItemSO queryItemSO;

    public void Inquire()
    {
        InvestigationEvents.InvokeQueryItemSelected(queryItemSO);
    }
}
