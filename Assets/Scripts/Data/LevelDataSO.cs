using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int Star;
}
[CreateAssetMenu(fileName = "new LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelDataSO : ScriptableObject
{
    [Searchable]
    public List<LevelData> LevelDataList;
    public int currentLevelHasPlayed = 0;
}
