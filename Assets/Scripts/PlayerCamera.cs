using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform playerTransform;
    float xAxisRotation = 0;
    float yAxisRotation = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xAxisRotation -= mouseY;
        xAxisRotation = Mathf.Clamp(xAxisRotation, -90, 90);
        yAxisRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xAxisRotation, 0, 0);
        playerTransform.localRotation = Quaternion.Euler(0, yAxisRotation, 0);

        //playerTransform.Rotate(Vector3.up * mouseX);

        //playerTransform.Rotate(Vector3.right * mouseY);
        //transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);

    }
}
