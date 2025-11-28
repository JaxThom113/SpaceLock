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
        
        previousRagdollState = startRagdoll; // need this to fix slow motion ragdoll bug
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
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }
}