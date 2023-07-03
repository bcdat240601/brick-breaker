using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : SceneLoaderConnect
{
    private readonly string GAME_OVER_SCENE_NAME = "GameOver";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ball triggered an event with the lose collider 
        if (other.name.ToLower() == "ball")
        {
            var gameSession = GameSession.Instance;
            
            // checks for game over
            if (gameSession.PlayerLives <= 0)
            {
                sceneLoaderChannel.RaiseLoadSceneByName(GAME_OVER_SCENE_NAME);
                return;
            }

            // deduces a game life from the player
            int playerLive = gameSession.PlayerLives;
            gameSession.SetPlayerLives(--playerLive);
            FixBallOnPaddleAfterLoss();
            
        }
    }

    private void FixBallOnPaddleAfterLoss()
    {
        var ball = FindObjectOfType<Ball>();
        ball.HasBallBeenShot = false;
    }
}
