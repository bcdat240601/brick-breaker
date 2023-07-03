using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : SetupBehaviour
{

    [SerializeField] protected UIChannel uiChannel;

    // state
    private static GameSession _instance;
    public static GameSession Instance => _instance;

    [SerializeField] protected int gameLevel;
    public int GameLevel => gameLevel;
    [SerializeField] protected int playerScore;
    public int PlayerScore => playerScore;
    [SerializeField] protected int playerLives = 3;
    public int PlayerLives => playerLives;
    [SerializeField] protected int pointsPerBlock = 100;
    public int PointsPerBlock => pointsPerBlock;
    [SerializeField] protected float gameSpeed = 1f;
    public float GameSpeed { get => gameSpeed; set => gameSpeed = value; }
    
    /**
     * Singleton implementation.
     */
    protected override void Awake() 
    {
        base.Awake();
        // this is not the first instance so destroy it!
        if (_instance != null && _instance != this)
        { 
            Destroy(this.gameObject);
            return;
        }
        
        _instance = this;
        SceneManager.activeSceneChanged += SubcribeChangeScene;
        SceneManager.sceneLoaded += ChangeScene;
        PotionManager.Instance.OnPotionApply += AddHeart;
    }

    private void ChangeScene(Scene arg0, LoadSceneMode arg1)
    {                
        GameSpeed = 1f;
        playerLives = 3;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetUIChannel();
    }

    protected virtual void GetUIChannel()
    {
        if (uiChannel != null) return;
        string path = "Channel/UIChannel";
        uiChannel = Resources.Load<UIChannel>(path);
        Debug.Log("Reset " + nameof(uiChannel) + " in " + GetType().Name);
    }

    private void SubcribeChangeScene(Scene arg0, Scene arg1)
    {
        SetInforForUI();        
    }

    protected void SetInforForUI()
    {
        Debug.Log("SetInfor");
        uiChannel.RaiseLevel(gameLevel);
        uiChannel.RaiseLives(playerLives);
        uiChannel.RaisePoint(playerScore);
    }

    private void AddHeart(PotionType obj)
    {
        if (obj != PotionType.Heart) return;
        if (PlayerLives >= 5) return;
        playerLives++;
        uiChannel.RaiseLives(PlayerLives);
    }
    public virtual void SetGameLevel(int level)
    {
        gameLevel = level;
        uiChannel.RaiseLevel(gameLevel);
    }
    public virtual void SetPlayerLives(int live)
    {
        playerLives = live;
        uiChannel.RaiseLives(playerLives);
    }


    /**
     * Update per-frame.
     */
    void Update()
    {
        Time.timeScale = this.GameSpeed;        
    }

    /**
     * Updates player score with given points and also updates the UI score. The total points that are
     * calculated is based on the basis value (this.PointsPerBlock).
     */
    public void AddToPlayerScore(int blockMaxHits)
    {
        playerScore += blockMaxHits * this.PointsPerBlock;
        uiChannel.RaisePoint(playerScore);
    }
}
