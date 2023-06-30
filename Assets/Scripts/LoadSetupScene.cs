using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSetupScene : SetupBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        LoadingSetupScene();
    }

    protected virtual void LoadingSetupScene()
    {        
        Scene scene = SceneManager.GetSceneByName("SetupScene");
        Debug.Log(scene.name);
        if (!scene.isLoaded)
        {
            SceneManager.LoadSceneAsync("SetupScene", LoadSceneMode.Additive);
            Debug.Log("loadsetup");
        }
    }
}
