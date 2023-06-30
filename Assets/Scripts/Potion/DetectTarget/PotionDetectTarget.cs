using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDetectTarget : DetectTargetByTriggerEnter
{
    protected override void BeginSendingDamage(Transform target)
    {
        PotionDamageSender potionDamageSender = transform.parent.GetComponentInChildren<PotionDamageSender>();
        potionDamageSender.SendDamage(target, 0);
    }
}
