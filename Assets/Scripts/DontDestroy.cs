using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : SetupBehaviour
{
    [HideInInspector]
    public string objectID;
    protected override void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        var dontDestroyObjects = Object.FindObjectsOfType<DontDestroy>();

        for (int i = 0; i < dontDestroyObjects.Length; i++)
        {
            if (dontDestroyObjects[i] != this && dontDestroyObjects[i].objectID == objectID)
            {
                Destroy(gameObject);
            }

        }
        DontDestroyOnLoad(gameObject);
    }
}