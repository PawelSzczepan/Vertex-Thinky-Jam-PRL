using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    Camera cm;
    private Animator anim;

    public enum Position
    {
        front,
        right,
        back,
        left,
        obj
    }

    Position state = Position.front;
    Position prevState;
    string prevCmName;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cm = Camera.main;

        anim.Play("Front");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            switch (state)
            {
                case Position.front:
                    Transition("Right", Position.right);
                    break;

                case Position.right:
                    Transition("Back", Position.back);
                    break;

                case Position.back:
                    Transition("Left", Position.left);
                    break;

                case Position.left:
                    Transition("Front", Position.front);
                    break;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            switch (state)
            {
                case Position.front:
                    Transition("Left", Position.left);
                    break;

                case Position.right:
                    Transition("Front", Position.front);
                    break;

                case Position.back:
                    Transition("Right", Position.right);
                    break;

                case Position.left:
                    Transition("Back", Position.back);
                    break;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (state == Position.obj)
            {
                InvestigationEvents.InvokeUnfocused();
                Transition(prevCmName, prevState);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = cm.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.GetComponent<ClickableObject>().OnClick();
            }
        }
    }

    public void Transition(string stateName, Position newState)
    {
        anim.Play(stateName);
        if (state != Position.obj)
            prevState = state;
        if (newState != Position.obj)
            prevCmName = stateName;
        state = newState;
    }
}
