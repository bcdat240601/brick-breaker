using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTargetByTriggerEnter : DetectTarget
{
    [Header("DetectTargetByTriggerEnter")]
    [SerializeField] protected Collider2D _collider2D;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetCollider();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {        
        if (!ConditionToDetect(collision)) return;
        BeginSendingDamage(collision.transform);
    }
    protected virtual bool ConditionToDetect(Collider2D collision)
    {
        // override some condition above
        return true;
    }
    protected virtual void GetCollider()
    {
        if (_collider2D != null) return;
        _collider2D = GetComponent<Collider2D>();
        _collider2D.isTrigger = true;
        Debug.Log("Reset " + this.GetType().Name + " in " + transform.name + " : " + nameof(_collider2D));
    }
}