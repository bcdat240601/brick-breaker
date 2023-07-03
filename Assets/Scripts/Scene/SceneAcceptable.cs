using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum PrimaryScene
{
    None,
    StartMenu,
    Level1,
    Level2,
    Level3,
    LevelMap,
    GameOver,
    InstructionsMenu
}
public enum SubScene
{
    None,
    SetUpScene,
    UI
}
[CreateAssetMenu(fileName = "SceneAcceptable", menuName = "ScriptableObjects/Scene/SceneAcceptable")]
public class SceneAcceptable : SerializedScriptableObject
{
    public Dictionary<PrimaryScene, List<SubScene>> SceneDic;
}
