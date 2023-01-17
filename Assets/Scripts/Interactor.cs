using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Manipulates interactables
public class Interactor : MonoBehaviour
{
    private Collider[] overlapResults = new Collider[50];
    private Interactable heldItem;

    public Bucket bucket;

    // Start is called before the first frame update
    void Start()
    {
        bucket.Pickup(transform);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnUse()
    {
        if (!heldItem) bucket.Use(transform);
        else
        {
            var shouldDrop = heldItem.Use(transform);
            if (shouldDrop) heldItem = null;
        }
    }

    public virtual void OnPickup()
    {
        //Drop the best item AFTER we've found the best item
        if(heldItem)
        {
            heldItem.Drop();
            heldItem = null;
            return;
        }
        
        //Get the interactable with the closest dot to the z axis
        int results = Physics.OverlapSphereNonAlloc(transform.position, 3f, overlapResults);

        float closestDot = -1f;
        Interactable bestItem = null;
        for (int i = 0; i < results; i++)
        {
            //Check if the object has the interactable component
            Interactable interactable = overlapResults[i].GetComponent<Interactable>();
            if (!interactable || interactable.pickedUp) continue;

            //test dot product
            var trans = transform;
            var dot = Vector3.Dot(trans.forward, (interactable.transform.position - trans.position).normalized);
            if (dot > closestDot)
            {
                closestDot = dot;
                bestItem = interactable;
            }
        }

        //Interact with the best item
        if (bestItem && bestItem != heldItem)
        {
            bestItem.Pickup(transform);
            heldItem = bestItem;
        }
        

        //Drop item
        // else if (heldItem)
        // {
        //     heldItem.Drop();
        //     heldItem = null;
        // }
    }
}