using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryItem : MonoBehaviour
{

    public void Inquire()
    {
        InvestigationEvents.InvokeQueryItemSelected(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Inquire();
        }
    }
}
