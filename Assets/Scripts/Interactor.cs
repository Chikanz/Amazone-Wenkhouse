using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Manipulates interactables
public class Interactor : MonoBehaviour
{
    private Collider[] overlapResults = new Collider[50];

    private Interactable heldItem;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnUse()
    {
        if(!heldItem) return;
        heldItem.Use(transform);
    }

    public virtual void OnPickup()
    {
        if (!heldItem)
        {
            //Get the interactable with the closest dot to the z axis

            int results = Physics.OverlapSphereNonAlloc(transform.position, 3f, overlapResults);

            float closestDot = -1f;
            Interactable bestItem = null;
            for (int i = 0; i < results; i++)
            {
                //Check if the object has the interactable component
                Interactable interactable = overlapResults[i].GetComponent<Interactable>();
                if(!interactable) continue;

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
            if (bestItem)
            {
                bestItem.Pickup(transform);
                heldItem = bestItem;
            }
        }

        //Drop item
        else if (heldItem)
        {
            heldItem.Drop();
            heldItem = null;
        }
    }
}