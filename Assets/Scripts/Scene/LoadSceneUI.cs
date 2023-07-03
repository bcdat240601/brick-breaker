using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneUI : SetupBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        LoadingSetupScene();
    }

    protected virtual void LoadingSetupScene()
    {
        Scene scene = SceneManager.GetSceneByName(SubScene.UI.ToString());
        Debug.Log(scene.name);
        if (!scene.isLoaded)
        {
            SceneManager.LoadSceneAsync(SubScene.UI.ToString(), LoadSceneMode.Additive);
        }
    }
}
