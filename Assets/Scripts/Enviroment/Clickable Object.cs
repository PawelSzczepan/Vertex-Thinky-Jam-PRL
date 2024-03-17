using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField] string cameraName;
    [SerializeField] bool cameraTransition = true;
    CameraManager cameraManager;

    private void Start()
    {
        cameraManager = CameraManager.instance;
    }


    public void OnClick()
    {
        if(cameraTransition)
            cameraManager.Transition(cameraName, CameraManager.Position.obj);

        onClick.Invoke();
    }
}
