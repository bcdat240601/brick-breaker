using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageSender : SetupBehaviour
{
    public virtual void SendDamage(Transform gameObject, int damage)
    {
        IDamageable damageableObject = gameObject.GetComponent<IDamageable>();
        if (damageableObject == null) return;
        SendDamage(damageableObject, damage);
    }
    protected virtual void SendDamage(IDamageable damageableObject, int damage)
    {
        damageableObject.TakeDamage(damage);
    }

}