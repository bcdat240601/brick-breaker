using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UIChannel", menuName = "ScriptableObjects/Channel/UIChannel")]
public class UIChannel : ScriptableObject
{
    public event Action<int> OnUpdateLives;
    public event Action<int> OnUpdateLevel;
    public event Action<int> OnUpdatePoint;

    public virtual void RaiseLives(int lives)
    {
        OnUpdateLives?.Invoke(lives);
    }

    public virtual void RaiseLevel(int level)
    {
        OnUpdateLevel?.Invoke(level);
    }
    public virtual void RaisePoint(int point)
    {
        OnUpdatePoint?.Invoke(point);
    }
}
