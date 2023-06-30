using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : SceneLoaderConnect
{
    private readonly string GAME_OVER_SCENE_NAME = "Scenes/GameOver";
    private readonly int NUMBER_OF_GAME_LEVELS = 3;
    
    // UI elements
    [SerializeField] int blocksCounter;

    public void IncrementBlocksCounter()
    {
        blocksCounter++;
    }
    
    public void DecrementBlocksCounter()
    {
        blocksCounter--;

        if (blocksCounter <= 0)
        {
            var gameSession = GameSession.Instance;
            
            // check for game over
            if (gameSession.GameLevel >= NUMBER_OF_GAME_LEVELS)
            {
                sceneLoaderChannel.RaiseLoadSceneByName(GAME_OVER_SCENE_NAME);
            }

            // increases game level
            ConfigManager.Instance.LevelDataSO.LevelDataList[gameSession.GameLevel - 1].Star = 3;
            if(ConfigManager.Instance.LevelDataSO.currentLevelHasPlayed < gameSession.GameLevel)
                ConfigManager.Instance.LevelDataSO.currentLevelHasPlayed = gameSession.GameLevel;
            gameSession.GameLevel++;
            sceneLoaderChannel.RaiseLoadNextScene();
        }
    }
    
}
