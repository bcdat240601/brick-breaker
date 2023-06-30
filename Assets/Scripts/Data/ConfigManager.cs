using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigManager : SetupBehaviour
{
    protected static ConfigManager instance;
    public static ConfigManager Instance => instance;

    public LevelDataSO LevelDataSO;
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Destroy(this.gameObject);
            Debug.LogError("there's 2 ConfigManager in the scene");
        }
        instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetLevelDataSO();
    }
    protected virtual void GetLevelDataSO()
    {
        if (LevelDataSO != null) return;
        string path = "LevelData/LevelData";
        LevelDataSO = Resources.Load<LevelDataSO>(path);
        Debug.Log("Reset " + nameof(LevelDataSO) + " in " + GetType().Name);
    }
}
