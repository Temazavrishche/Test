using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController ch;
    public float speed;
    public bool jump;

    private void Start()
    {
        ch = GetComponent<CharacterController>();
    }
    private void Update()
    {
        ch.Move(transform.forward * speed * Time.deltaTime);
    }
}
