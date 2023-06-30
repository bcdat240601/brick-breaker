using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderConnect : SetupBehaviour
{
    [SerializeField] protected SceneLoaderChannel sceneLoaderChannel;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetSceneLoaderChannel();
    }

    protected virtual void GetSceneLoaderChannel()
    {
        if (sceneLoaderChannel != null) return;
        string path = "Channel/SceneLoaderChannel";
        sceneLoaderChannel = Resources.Load<SceneLoaderChannel>(path);
        Debug.Log("Reset " + nameof(sceneLoaderChannel) + " in " + GetType().Name);
    }
}
