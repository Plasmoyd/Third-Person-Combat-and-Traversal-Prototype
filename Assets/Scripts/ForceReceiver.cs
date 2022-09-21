using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;

    private void Update()
    {
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // we are performing addition instead of subtraction because gravity is negative value.
        }
    }

    public Vector3 Movement() => Vector3.up * verticalVelocity; // this is the force that represents gravity
}
