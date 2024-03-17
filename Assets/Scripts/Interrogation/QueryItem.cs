using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryItem : MonoBehaviour
{
    public new string name;

    public void Inquire()
    {
        InvestigationEvents.InvokeQueryItemSelected(this);
    }
}
