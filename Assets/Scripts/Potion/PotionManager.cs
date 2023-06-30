using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType
{
    Heart = 1,
    Gear = 2,
    BlueBottle = 3,
    EmptyBottle = 4,
}
[Serializable]
public class PotionApply
{
    public PotionType PotionType;
    public Timer Timer;
    public bool isHavingDuration;
}
public class PotionManager : SetupBehaviour
{
    [SerializeField] protected List<PotionApply> potionApplies;
    public event Action<PotionType> OnEffectTimeout;
    public event Action<PotionType> OnPotionApply;
    public event Action OnRemoveAllEffect;
    protected static PotionManager instance;
    public static PotionManager Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Destroy(this.gameObject);
            Debug.LogError("there's 2 PotionManager in the scene");
        }
        instance = this;
    }
    protected virtual void Update()
    {
        CheckBuffTimeout();
    }
    protected virtual void CheckBuffTimeout()
    {
        if (potionApplies.Count == 0) return;
        for (int i = 0; i < potionApplies.Count; i++)
        {
            if (!potionApplies[i].isHavingDuration)
                continue;
            if (potionApplies[i].Timer.CheckTimer())
            {
                OnEffectTimeout?.Invoke(potionApplies[i].PotionType);
                potionApplies.Remove(potionApplies[i]);
                i--;
            }
        }
    }

    public virtual void AddEffect(PotionType potionType)
    {
        PotionApply potionApply = new PotionApply();
        potionApply.Timer = new Timer();
        switch (potionType)
        {
            case PotionType.Heart:
                potionApply.isHavingDuration = false;
                potionApply.PotionType = potionType;
                break;
            case PotionType.Gear:
                if (FindPotionInList(potionType, out int index))
                {
                    potionApplies[index].Timer.RefreshTimer();
                }
                else
                {
                    potionApply.isHavingDuration = true;
                    potionApply.Timer.SetCooldownTime(10f);
                    potionApply.PotionType = potionType;
                    potionApplies.Add(potionApply);
                }                
                break;
            case PotionType.BlueBottle:
                potionApply.isHavingDuration = true;
                potionApply.Timer.SetCooldownTime(10f);
                potionApply.PotionType = potionType;
                potionApplies.Add(potionApply);
                break;
            case PotionType.EmptyBottle:
                potionApplies.Clear();
                OnRemoveAllEffect?.Invoke();
                break;
            default:
                break;
        }
        OnPotionApply?.Invoke(potionType);
    }
    protected virtual bool FindPotionInList(PotionType potionType, out int index)
    {
        for (int i = 0; i < potionApplies.Count; i++)
        {
            if (potionApplies[i].PotionType == potionType)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }
}
