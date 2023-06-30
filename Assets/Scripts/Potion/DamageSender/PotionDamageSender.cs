using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDamageSender : DamageSender
{
    [SerializeField] protected PotionType buffType;
    public override void SendDamage(Transform gameObject, int amount)
    {
        IBuffReceiver buffObject = gameObject.GetComponent<IBuffReceiver>();
        if (buffObject == null) return;
        SendDamage(buffObject, amount);
    }
    protected virtual void SendDamage(IBuffReceiver buffObject, int amount)
    {
        buffObject.ReceiveBuff(buffType);
        PotionSpawner.Instance.Despawn(transform.parent);
    }
}
