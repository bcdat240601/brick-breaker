using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    [SerializeField] protected float timer = 0;
    public float Timmer => timer;
    [SerializeField] protected float cooldownTime;

    public void SetCooldownTime(float cooldownTime)
    {
        this.cooldownTime = cooldownTime;
        StartTimer();
    }
    public void StartTimer()
    {
        timer = Time.time + cooldownTime;
    }
    public bool CheckTimer()
    {
        if (Time.time >= timer) return true;
        return false;
    }
    public void RefreshTimer()
    {
        float timeToRefresh = cooldownTime - (timer - Time.time);
        timer += timeToRefresh;
    }
    public void CancelTimer()
    {
        timer = Time.time;
    }
    public float GetTimeRemain()
    {
        float timeRemain = timer - Time.time;
        return timeRemain;
    }

}