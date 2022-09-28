using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;
    [SerializeField] NavMeshAgent agent;

    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;

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

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if(impact.sqrMagnitude < .2f*.2f && agent != null)
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }
    }

    public Vector3 Movement() => impact + Vector3.up * verticalVelocity; // this is the force that represents gravity

    public void AddForce(Vector3 force)
    {
        impact += force;
        if(agent != null)
        {
            agent.enabled = false;
        }
    }
}
