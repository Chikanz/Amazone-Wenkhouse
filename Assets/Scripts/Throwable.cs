using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Interactable
{
    public float throwForce = 1000;
    public float upForce = 300;
    protected bool thrown = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Use(Transform cameraRoot)
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
    }

    public override void Pickup(Transform cameraRoot)
    {
        base.Pickup(cameraRoot);
        
        thrown = false;
    }
}