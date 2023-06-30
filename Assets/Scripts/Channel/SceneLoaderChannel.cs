using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SceneLoaderChannel", menuName = "ScriptableObjects/Channel/SceneLoaderChannel")]
public class SceneLoaderChannel : ScriptableObject
{
    public event Action OnLoadNextScene;
    public event Action<string> OnLoadSceneByName;
    public event Action OnLoadStartScene;
    public event Action OnQuit; 

    public virtual void RaiseLoadNextScene()
    {
        OnLoadNextScene?.Invoke();
    }

    public virtual void RaiseLoadSceneByName(string name)
    {        
        OnLoadSceneByName?.Invoke(name);
    }
    public virtual void RaiseLoadStartScene()
    {
        OnLoadStartScene?.Invoke();
    }
    public virtual void RaiseQuit()
    {
        OnQuit?.Invoke();
    }
}
