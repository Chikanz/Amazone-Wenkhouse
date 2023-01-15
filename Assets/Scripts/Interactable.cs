using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Vector3 defaultScale = Vector3.zero;
    protected Rigidbody RB;
    protected bool pickedUp = false;
    new protected Collider collider;

    public int? HeldByPlayer = null;
    private Player heldBy;
    
    protected virtual void Start()
    {
        defaultScale = transform.localScale;
        RB = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Pickup(Transform cameraRoot)
    {
        //Get anchor transform by searching children for "Cam Anchor" name
        Transform t = transform;
        Transform anchor = t.Find("Cam Anchor");
        
        collider.enabled = false;
        if(RB)
        {
            RB.velocity = Vector3.zero;
            RB.isKinematic = true;
        }

        t.SetParent(cameraRoot);
        t.localPosition = anchor.localPosition;
        t.localRotation = anchor.localRotation;
        t.localScale = anchor.localScale;

        pickedUp = true;
        
        //Set HeldByPlayer to the player's ID
        var player = cameraRoot.parent.GetComponentInChildren<Player>();
        HeldByPlayer = player.PlayerID;
        heldBy = player;
    }

    public virtual void Use(Transform cameraRoot)
    {
        
    }

    public virtual void Drop()
    {
        transform.SetParent(null);
        
        //Raycast to find the floor from current position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            //Set position to the hit point
            transform.position = hit.point;
            transform.rotation = Quaternion.identity;
            ResetScale();
        }
        GetComponent<Collider>().enabled = true;
        pickedUp = false;
        HeldByPlayer = null;
    }

    protected void ResetScale()
    {
        transform.localScale = defaultScale;
    }

    protected void FinishedTask()
    {
        if(heldBy) heldBy.GetComponent<JEFF>().TaskCompleted();
    }
}
