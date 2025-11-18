using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos; // position where held object goes when BringIn()
    public Transform outPos; // position where held object goes when ThrowOut()
    public LayerMask holdLayerMask; // get Player layer to avoid raycast hitting player character collider
    public float pickUpRange = 5f;
    public float smoothTime = 0.1f;

    private GameObject heldObj;
    private Rigidbody heldObjRb; 
    private int LayerNumber;
    private Vector3 moveVelocity; // persistent velocity storage
    private bool inOrOut = false; // false if out, true if in
    private Vector3 previousPosition;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("PickUp");
    }

    void Update()
    {
        if (heldObj != null)
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                inOrOut = false;
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                inOrOut = true;
            }
        }
       
        if (Input.GetMouseButton(0)) // press and hold to pick up
        {
            // player is not holding an object
            if (heldObj == null)
            {
                // raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, holdLayerMask))
                {
                    //Debug.Log("Hit: " + hit.transform.name + " with tag: " + hit.transform.tag);

                    if (hit.transform.gameObject.tag == "PickUp")
                    {
                        inOrOut = false;
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }

            // player is holding object
            if (heldObj != null)
            {
                MoveObject(); // keep object position at holdPos
            }
        }
        else
        {
            if (heldObj != null)
            {
                StopClipping(); //prevents object from clipping through walls
                DropObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            outPos.position = heldObjRb.transform.position;
            previousPosition = heldObj.transform.position;
        }
    }

    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false); // enable collision with player
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        
        // calculate and apply velocity based on movement
        Vector3 throwVelocity = (heldObj.transform.position - previousPosition) / Time.deltaTime;
        heldObjRb.velocity = throwVelocity;
        
        heldObj.transform.parent = null; // unparent object
        heldObj = null; // undefine game object
    }

    void MoveObject()
    {
        // store previous position before moving
        previousPosition = heldObj.transform.position;

        if (inOrOut)
        {
            // object is "brought in"
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true); // disable collision with player

            heldObjRb.transform.position = Vector3.SmoothDamp(
                heldObj.transform.position,
                holdPos.position,
                ref moveVelocity,
                smoothTime
            );
        }
        else
        {
            // object is "thrown out"
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false); // enable collision with player

            heldObjRb.transform.position = Vector3.SmoothDamp(
                heldObj.transform.position,
                outPos.position,
                ref moveVelocity,
                smoothTime
            );
        }
    }

    void StartClipping()
    {
        heldObj.layer = LayerNumber; //change the object layer to the holdLayer
    }

    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); // distance from holdPos to the camera
        
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); 
        }
    }
}
