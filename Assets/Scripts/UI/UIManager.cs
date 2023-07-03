using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SetupBehaviour
{
    [SerializeField] protected TextMeshProUGUI playerScoreText;
    [SerializeField] protected TextMeshProUGUI gameLevelText;
    [SerializeField] protected TextMeshProUGUI playerLivesText;
    [SerializeField] protected UIChannel uiChannel;

    protected override void Awake()
    {
        base.Awake();
        uiChannel.OnUpdateLevel += UpdateLevel;
        uiChannel.OnUpdateLives += UpdatePlayerLives;
        uiChannel.OnUpdatePoint += UpdatePoints;

    }
    protected virtual void OnDestroy()
    {
        uiChannel.OnUpdateLevel -= UpdateLevel;
        uiChannel.OnUpdateLives -= UpdatePlayerLives;
        uiChannel.OnUpdatePoint -= UpdatePoints;
    }

    protected virtual void UpdatePoints(int value)
    {
        playerScoreText.text = value.ToString();
    }

    protected virtual void UpdatePlayerLives(int value)
    {
        playerLivesText.text = value.ToString();
    }

    protected virtual void UpdateLevel(int value)
    {
        gameLevelText.text = value.ToString();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetPlayerScoreText();
        GetGameLevel();
        GetPlayerLives();
        GetUIChannel();
    }

    protected virtual void GetUIChannel()
    {
        if (uiChannel != null) return;
        string path = "Channel/UIChannel";
        uiChannel = Resources.Load<UIChannel>(path);
        Debug.Log("Reset " + nameof(uiChannel) + " in " + GetType().Name);
    }

    protected virtual void GetPlayerScoreText()
    {
        if (playerScoreText != null) return;
        playerScoreText = transform.Find("ScorePoints").GetComponent<TextMeshProUGUI>();
        Debug.Log("Reset " + nameof(playerScoreText) + " in " + GetType().Name);
    }
    protected virtual void GetGameLevel()
    {
        if (gameLevelText != null) return;
        gameLevelText = transform.Find("GameLevel").GetComponent<TextMeshProUGUI>();
        Debug.Log("Reset " + nameof(gameLevelText) + " in " + GetType().Name);
    }
    protected virtual void GetPlayerLives()
    {
        if (playerLivesText != null) return;
        playerLivesText = transform.Find("PlayerLives").GetComponent<TextMeshProUGUI>();
        Debug.Log("Reset " + nameof(playerLivesText) + " in " + GetType().Name);
    }

}
