using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public CharacterController controller;
    public CapsuleCollider enemyCollider;
    public Transform player;
    public float speed = 8f;
    public float gravity = -50f;
    public float stoppingDistance = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    Vector3 velocity;
    bool isGrounded;

    private Animator animator;
    private RagdollEnabler ragdollEnabler;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        ragdollEnabler = GetComponent<RagdollEnabler>();
        enemyCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // move direction
        Vector3 move = (player.position - transform.position).normalized;
        move.y = 0f;

        // only move toward player if further than stoppingDistance
        if (distance > stoppingDistance)
        {
            animator.SetBool("Running", true);

            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            controller.Move(move * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PickUp")
        {
            // activate ragdoll if colliding with PickUp tagged object
            if (ragdollEnabler != null)
            {
                ragdollEnabler.startRagdoll = true;
            }
        }
    }
}
