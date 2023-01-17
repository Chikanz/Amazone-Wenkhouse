using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Interactable
{
    public float throwForce = 1000;
    public float upForce = 300;
    public bool thrown { get; protected set; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool Use(Transform cameraRoot)
    {
        base.Use(cameraRoot);

        pickedUp = false;
        ResetScale();
        transform.SetParent(null);
        collider.enabled = true;
        RB.isKinematic = false;
        RB.AddForce((cameraRoot.forward * throwForce) + (cameraRoot.up * upForce));

        IEnumerator WaitForThrown()
        {
            yield return new WaitForFixedUpdate();
            thrown = true;
        }
        
        StartCoroutine(WaitForThrown());

        return true; //Drop this bitch
    }

    public override void Pickup(Transform cameraRoot)
    {
        base.Pickup(cameraRoot);
        
        thrown = false;
    }
}