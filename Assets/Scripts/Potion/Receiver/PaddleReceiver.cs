using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleReceiver : SetupBehaviour, IBuffReceiver
{
    public void ReceiveBuff(PotionType potionType)
    {
        PotionManager.Instance.AddEffect(potionType);
    }
}
