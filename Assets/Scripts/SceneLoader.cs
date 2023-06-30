﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SetupBehaviour
{
    [SerializeField] protected SceneLoaderChannel SceneLoaderChannel;
    [SerializeField] protected string SETUP_SCENE;
    [SerializeField] protected List<Scene> sceneToUnload;
    [SerializeField] protected int currentSceneIndex;

    protected override void Awake()
    {
        base.Awake();
        SceneLoaderChannel.OnLoadNextScene += LoadNextScene;
        SceneLoaderChannel.OnLoadSceneByName += LoadSceneByName;
        SceneLoaderChannel.OnLoadStartScene += LoadStartScene;
        SceneLoaderChannel.OnQuit += Quit;
        SceneManager.activeSceneChanged += HideMouse;
        SETUP_SCENE = SceneManager.GetSceneByBuildIndex(0).name;
    }

    private void HideMouse(Scene arg0, Scene arg1)
    {
        HideMouse();
    }

    protected virtual void OnDestroy()
    {
        SceneLoaderChannel.OnLoadNextScene -= LoadNextScene;
        SceneLoaderChannel.OnLoadSceneByName -= LoadSceneByName;
        SceneLoaderChannel.OnLoadStartScene -= LoadStartScene;
        SceneLoaderChannel.OnQuit -= Quit;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetSceneLoaderChannel();
    }

    protected virtual void GetSceneLoaderChannel()
    {
        if (SceneLoaderChannel != null) return;
        string path = "Channel/SceneLoaderChannel";
        SceneLoaderChannel = Resources.Load<SceneLoaderChannel>(path);
        Debug.Log("Reset " + nameof(SceneLoaderChannel) + " in " + GetType().Name);
    }

    // loads next scene based on the scene ordering defined on Unity > build settings
    public void LoadNextScene()
    {
        AddScenesToUnload();
        currentSceneIndex++;
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Additive);
        UnloadScenes();
    }

    // loads scene by its name
    public void LoadSceneByName(string sceneName)
    {
        AddScenesToUnload();        
        SceneManager.LoadSceneAsync(sceneName: sceneName, LoadSceneMode.Additive);
        currentSceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        UnloadScenes();
    }

    // always the 0 indexed scene
    public void LoadStartScene()
    {
        // FindObjectOfType<GameState>().ResetState();
        Debug.Log("LoadStart");
        AddScenesToUnload();
        currentSceneIndex = 0;
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Additive);
        UnloadScenes();
    }

    public void Quit()
    {
        Application.Quit();
    }

    /**
    * Hides the mouse cursor.
    */

    protected virtual void HideMouse()
    {
        Debug.Log("hideMouse");
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "LevelMap")
            {
                return;
            }
        }
        Cursor.visible = false;
    }

    private void AddScenesToUnload()
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != SETUP_SCENE)
            {
                Debug.Log("Added scene to unload = " + scene.name);
                //Add the scene to the list of the scenes to unload
                sceneToUnload.Add(scene);
            }
        }
    }
    private void UnloadScenes()
    {
        if (sceneToUnload != null)
        {
            for (int i = 0; i < sceneToUnload.Count; ++i)
            {
                //Unload the scene asynchronously in the background
                SceneManager.UnloadSceneAsync(sceneToUnload[i]);
            }
        }
        sceneToUnload.Clear();
    }
}
