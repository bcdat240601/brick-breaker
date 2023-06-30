using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using UnityEditor;

public class Ball : SetupBehaviour
{
    // constants
    private const int MOUSE_PRIMARY_BUTTON = 0;
    [SerializeField] protected ObjectPlay objectPlay;
    // fields
    [SerializeField] private Vector2 initialBallSpeed = new Vector2(4f, 12f);
    [SerializeField] private float bounceRandomnessFactor = 0.5f;

    //[SerializeField, FolderPath(ParentFolder = "Assets")] string path;

    [SerializeField] protected List<AudioClip> bumpAudioClips;
    [SerializeField] protected float speed;
    [SerializeField] protected float defaultSpeed = 1f;
    [SerializeField] protected float bluePotionStack;

    private Vector2 _initialDistanceToTopOfPaddle;
    private Rigidbody2D _rigidBody2D;
    private AudioSource _audioSource;

    // properties
    public Vector2 InitialBallSpeed { get; set; }
    public Paddle Paddle { get; set; }
    public bool HasBallBeenShot { get; set; } = false;

    protected override void Awake()
    {
        base.Awake();
        var ballPosition = transform.position;
        var paddlePosition = objectPlay.Paddle.transform.position;

        _initialDistanceToTopOfPaddle = ballPosition - paddlePosition;  // assumes ball always starts on TOP of the paddle        
        PotionManager.Instance.OnPotionApply += ApplyPotion;
        PotionManager.Instance.OnEffectTimeout += RemovePotion;
        PotionManager.Instance.OnRemoveAllEffect += RemoveAllBluePotion;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetRigidBody();
        GetAudioSource();
        SetAudioClips();
        GetObjectPlay();
    }

    protected virtual void GetObjectPlay()
    {
        if (objectPlay != null) return;
        objectPlay = GetComponentInParent<ObjectPlay>();
        Debug.Log("Reset " + nameof(objectPlay) + " in " + GetType().Name);
    }

    protected virtual void GetAudioSource()
    {
        if (_audioSource != null) return;
        _audioSource = GetComponent<AudioSource>();
        Debug.Log("Reset " + nameof(_audioSource) + " in " + GetType().Name);
    }

    protected virtual void GetRigidBody()
    {
        if (_rigidBody2D != null) return;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        Debug.Log("Reset " + nameof(_rigidBody2D) + " in " + GetType().Name);
    }

    private AudioClip[] audioClips;

    public void SetAudioClips()
    {
        bumpAudioClips.Clear();
        string path = "Audio/bump";
        string fullPath = $"{Application.dataPath}/{path}";
        if (!System.IO.Directory.Exists(fullPath))
        {
            return;
        }

        var folders = new string[] { $"Assets/{path}" };
        var guids = AssetDatabase.FindAssets("t:AudioClip", folders);

        var newSprites = new AudioClip[guids.Length];

        bool mismatch;
        if (audioClips == null)
        {
            mismatch = true;
            audioClips = newSprites;
        }
        else
        {
            mismatch = newSprites.Length != audioClips.Length;
        }

        for (int i = 0; i < newSprites.Length; i++)
        {
            path = AssetDatabase.GUIDToAssetPath(guids[i]);
            newSprites[i] = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            mismatch |= (i < audioClips.Length && audioClips[i] != newSprites[i]);
        }

        if (mismatch)
        {
            audioClips = newSprites;
            Debug.Log($"{name} Audio list updated.");
        }

        foreach (AudioClip obj in audioClips)
        {
            bumpAudioClips.Add(obj);
        }
        Array.Clear(audioClips, 0, audioClips.Length);
    }

    private void Start()
    {        
        defaultSpeed = GameSession.Instance.GameSpeed;
        speed = defaultSpeed;
    }

    protected virtual void ApplyPotion(PotionType obj)
    {
        if (obj == PotionType.BlueBottle)
            ApplyOneBluePotion();
    }
    protected virtual void RemovePotion(PotionType obj)
    {
        if (obj == PotionType.BlueBottle)
            RemoveOneBluePotion();
    }

    private void Update()
    {        
        // if ball has been shot, no locking or shooting it again!
        if (HasBallBeenShot) return;
        
        var hasMouseClick = Input.GetMouseButtonDown(MOUSE_PRIMARY_BUTTON);
        var paddlePosition = objectPlay.Paddle.transform.position;
            
        FixBallOnTopOfPaddle(paddlePosition, _initialDistanceToTopOfPaddle);
        ShootBallOnClick(initialBallSpeed, hasMouseClick);
    }
    
    /**
     * Fixes the ball on top of the paddle before the first mouse click.
     */
    public void FixBallOnTopOfPaddle(Vector2 paddlePosition, Vector2 distanceToPaddle)
    {
        transform.position = paddlePosition + distanceToPaddle;
    }
    
    /**
     * Shoots the ball for the first time upon the first mouse click.
     */
    public void ShootBallOnClick(Vector2 initialBallSpeed, bool hasMouseClick)
    {
        if (!hasMouseClick) return;
        
        HasBallBeenShot = true;
        _rigidBody2D.velocity = initialBallSpeed;
    }

    /**
     * Computes a random vector to add to the ball's velocity vector in order to avoid
     * repetitive ball collisions throughout the game.
     */
    public Vector2 GetRandomVelocityBounce()
    {

        var randomVelocityX = Random.Range(0, this.bounceRandomnessFactor);
        var randomVelocityY = Random.Range(0, this.bounceRandomnessFactor);
        
        return new Vector2(randomVelocityX, randomVelocityY);
    }
    
    /**
     * Randomly plays ball collision sounds.
     */
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!HasBallBeenShot) return;  // ball must have been shot first
        
        var randomBumpAudioIndex = Random.Range(0, bumpAudioClips.Count);
        var signVelocityY = Math.Sign(_rigidBody2D.velocity.y);
        var signVelocityX = Math.Sign(_rigidBody2D.velocity.x);
        
        var correctVelocityY = _rigidBody2D.velocity.y;
        var correctVelocityX = _rigidBody2D.velocity.x;
        
        var bumpAudio = bumpAudioClips[randomBumpAudioIndex];
            
        _audioSource.PlayOneShot(bumpAudio);
        // _rigidBody2D.velocity += GetRandomVelocityBounce();

        if (Math.Abs(_rigidBody2D.velocity.y) < 4f) correctVelocityY = 4f * signVelocityY;
        if (Math.Abs(_rigidBody2D.velocity.x) < 4f) correctVelocityX = 4f * signVelocityX;
        
        _rigidBody2D.velocity = new Vector2(correctVelocityX, correctVelocityY);
    }
    protected virtual void ApplyOneBluePotion()
    {
        if (bluePotionStack >= 5) return;        
        speed -= defaultSpeed * 0.1f;
        GameSession.Instance.GameSpeed = speed;
        bluePotionStack++;
    }
    protected virtual void RemoveOneBluePotion()
    {
        if (bluePotionStack <= 0) return;
        speed += defaultSpeed * 0.1f;
        GameSession.Instance.GameSpeed = speed;
        bluePotionStack--;
    }
    protected virtual void RemoveAllBluePotion()
    {
        GameSession.Instance.GameSpeed = defaultSpeed;
    }

}
