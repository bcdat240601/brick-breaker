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
        Scene scene = SceneManager.GetSceneByName(SubScene.SetUpScene.ToString());
        Debug.Log(scene.name);
        if (!scene.isLoaded)
        {
            SceneManager.LoadSceneAsync(SubScene.SetUpScene.ToString(), LoadSceneMode.Additive);
        }
    }
}
