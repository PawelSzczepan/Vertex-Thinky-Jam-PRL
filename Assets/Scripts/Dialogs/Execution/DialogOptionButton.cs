using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class DialogOptionButton : MonoBehaviour, IPointerEnterHandler
{
    public event Action<DialogOptionButton> onClick;
    public event Action<DialogOptionButton> onHover;

    public void OnClick()
    {
        onClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover?.Invoke(this);
    }
}
