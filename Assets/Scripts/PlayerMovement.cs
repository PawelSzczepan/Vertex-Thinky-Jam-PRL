using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Vector3 move;
    float height; 
    CharacterController characterController;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        height = transform.position.y;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        move = transform.right * x + transform.forward * z;
        
        characterController.Move(move * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
