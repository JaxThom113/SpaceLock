using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    public CharacterController enemyController;
    public CapsuleCollider enemyCollider;
    public Animator animator;
    public Transform ragdollRoot;
    public bool startRagdoll;
    public Rigidbody[] rigidbodies;

    private CharacterJoint[] joints;
    private Collider[] colliders;
    private bool previousRagdollState;

    void Awake()
    {
        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();
        
        previousRagdollState = startRagdoll;
        
        if (startRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }

    void Update()
    {
        // only call these methods when the state changes
        if (startRagdoll != previousRagdollState)
        {
            if (startRagdoll)
            {
                EnableRagdoll();
            }
            else
            {
                EnableAnimator();
            }
            
            previousRagdollState = startRagdoll;
        }
    }

    void EnableRagdoll()
    {
        GetComponent<EnemyMovement>().enabled = false;
        enemyController.enabled = false;
        enemyCollider.enabled = false;

        // disable animator, enable collisions on all limbs
        animator.enabled = false;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = false; // Set to false to reduce jitter
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false; // Important: make sure rigidbodies are not kinematic
            rigidbody.velocity = Vector3.zero; // Now only called ONCE
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }

    void EnableAnimator()
    {
        GetComponent<EnemyMovement>().enabled = true;
        enemyController.enabled = true;
        enemyCollider.enabled = true;

        // enable animator, disable collisions on limbs
        animator.enabled = true;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = false;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true; // Make kinematic when not ragdolling
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }
}