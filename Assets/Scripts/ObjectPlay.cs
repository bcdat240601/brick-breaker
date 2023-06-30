using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlay : SetupBehaviour
{
    [SerializeField] protected Paddle paddle;
    public Paddle Paddle => paddle;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetPaddle();
    }

    protected virtual void GetPaddle()
    {
        if (paddle != null) return;
        paddle = GetComponentInChildren<Paddle>();
        Debug.Log("Reset " + nameof(paddle) + " in " + GetType().Name);
    }
}
