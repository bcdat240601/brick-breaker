using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SetupBehaviour
{
    [SerializeField] protected SceneAcceptable sceneAcceptable;
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
        GetSceneAcceptable();
    }

    protected virtual void GetSceneAcceptable()
    {
        if (sceneAcceptable != null) return;
        string path = "Scene/SceneAcceptable";
        sceneAcceptable = Resources.Load<SceneAcceptable>(path);
        Debug.Log("Reset " + nameof(sceneAcceptable) + " in " + GetType().Name);
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
        currentSceneIndex++;
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByBuildIndex(currentSceneIndex);
        AddScenesToUnload(scene.name);
        UnloadScenes();
    }

    // loads scene by its name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentSceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        AddScenesToUnload(sceneName);        
        UnloadScenes();
    }

    // always the 0 indexed scene
    public void LoadStartScene()
    {
        // FindObjectOfType<GameState>().ResetState();
        currentSceneIndex = 0;
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByBuildIndex(currentSceneIndex);
        AddScenesToUnload(scene.name);
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
    protected virtual void AddScenesToUnload(string sceneName)
    {         
        // Parse the Next Scene name To PrimaryScene enum
        Enum.TryParse(sceneName, out PrimaryScene primaryScene);
        if(primaryScene == PrimaryScene.None)
        {
            // return if the next scene is not found
            Debug.LogError("Invalid Scene");
            return;
        }
        // Get the list of sub scene of next scene
        sceneAcceptable.SceneDic.TryGetValue(primaryScene, out List<SubScene> subScene);
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            // Return this function if the active scene is the next scene because
            // we load the next scene additively first before unload previous scene
            if (scene.name == sceneName) return;

            Enum.TryParse(scene.name, out SubScene result);
            if(result == SubScene.None)
            {
                Debug.Log("Added scene to unload = " + scene.name);
                //Add the scene to the list of the scenes to unload
                sceneToUnload.Add(scene);
                continue;
            }
            else
            {
                if(subScene == null)
                {
                    Debug.Log("Added scene to unload = " + scene.name);
                    //Add the scene to the list of the scenes to unload
                    sceneToUnload.Add(scene);
                    continue;
                }

                if (!subScene.Contains(result))
                {
                    Debug.Log("Added scene to unload = " + scene.name);
                    //Add the scene to the list of the scenes to unload
                    sceneToUnload.Add(scene);
                }
            }                       
        }
    }
    private void UnloadScenes()
    {
        Debug.Log("StartUnload");
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
