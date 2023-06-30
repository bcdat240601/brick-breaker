using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        LoadComponents();
        ResetValue();
    }

    protected virtual void Reset()
    {
        LoadComponents();
        ResetValue();
    }

    protected virtual void LoadComponents()
    {
        // for override
    }

    protected virtual void ResetValue()
    {
        // for override
    }
}
