using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : SetupBehaviour
{
    public static ConfigManager Instance { get; private set; }
    public LevelDataSO LevelDataSO;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

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
