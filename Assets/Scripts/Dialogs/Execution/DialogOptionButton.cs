using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogOptionButton : MonoBehaviour
{
    public event Action<DialogOptionButton> onClick;

    public void OnClick()
    {
        onClick?.Invoke(this);
    }
}
