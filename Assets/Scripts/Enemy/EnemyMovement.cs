using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    public CharacterController controller;
    public CapsuleCollider enemyCollider;
    public Transform player;
    public bool chase;
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

    private float damageTimer = 0f;
    private float damageInterval = 0.25f; // damage every 1/4 second
    private int damageAmount = 5;
    private bool isCollidingWithPlayer = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        ragdollEnabler = GetComponent<RagdollEnabler>();
        enemyCollider = GetComponent<CapsuleCollider>();
    }

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

        if (chase == true)
        {
            animator.SetBool("Running", true);

            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            controller.Move(move * speed * Time.deltaTime);
        }   

        if (isCollidingWithPlayer)
        {
            damageTimer += Time.deltaTime;
            
            // check if time interval has passed before dealing damage again
            if (damageTimer >= damageInterval)
            {
                DamagePlayer();
                damageTimer = 0f;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PickUp" || collision.gameObject.tag == "Hazard")
        {
            // activate ragdoll if colliding with PickUp tagged object
            if (ragdollEnabler != null)
            {
                ragdollEnabler.startRagdoll = true;
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            isCollidingWithPlayer = true;
            damageTimer = 0f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isCollidingWithPlayer = false;
            damageTimer = 0f;
        }
    }

    void DamagePlayer()
    {
        // get healthbar component
        HealthBar healthBar = FindObjectOfType<HealthBar>();
        
        if (healthBar != null)
        {
            int currentHealth = (int)healthBar.slider.value;
            currentHealth -= damageAmount;
            
            // when the player "dies" send them back to main menu
            if (currentHealth <= 0)
            {
                SceneManager.LoadScene(0);
                Cursor.lockState = CursorLockMode.None;
            }
            
            healthBar.SetHealth(currentHealth);
        }
    }
}
