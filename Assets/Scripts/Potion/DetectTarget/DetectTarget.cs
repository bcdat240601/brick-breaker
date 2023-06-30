using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class DetectTarget : SetupBehaviour
{
    [Header("DetectTarget")]
    [SerializeField] protected Rigidbody2D rb;
    protected override void LoadComponents()
    {
        GetRigidBody();
    }

    protected virtual void GetRigidBody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        //this will make the rigidbody detect target even the target is still touching but not moving
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;


        Debug.Log("Reset " + this.GetType().Name + " in " + transform.parent.name + " : " + nameof(rb));
    }
    protected virtual void BeginSendingDamage(Transform target)
    {
        // do something
    }

}