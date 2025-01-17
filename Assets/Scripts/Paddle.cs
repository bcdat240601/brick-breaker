﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    public float minRelativePosX = 1f;  // assumes paddle size of 1 relative unit
    
    [SerializeField]
    public float maxRelativePosX = 15f;  // assumes paddle size of 1 relative unit
    
    [SerializeField]
    public float fixedRelativePosY = .64f;  // paddle does not move on the Y directiob
    
    // Unity units of the WIDTH of the screen (e.g. 16)
    [SerializeField]
    public float screenWidthUnits = 16;

    private void Awake()
    {
        PotionManager.Instance.OnPotionApply += ApplyGear;
        PotionManager.Instance.OnEffectTimeout += RemoveGear;
        PotionManager.Instance.OnRemoveAllEffect += RemoveGear;
    }

    private void ApplyGear(PotionType obj)
    {
        if (obj == PotionType.Gear)
            transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
    }
    private void RemoveGear(PotionType obj)
    {
        if (obj == PotionType.Gear)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }
    protected virtual void RemoveGear()
    {
        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        float startPosX = ConvertPixelToRelativePosition(screenWidthUnits / 2, Screen.width);
        transform.position = GetUpdatedPaddlePosition(startPosX);
    } 

    // Update is called once per frame
    void Update()
    {
        var relativePosX = ConvertPixelToRelativePosition(pixelPosition: Input.mousePosition.x, Screen.width);
        transform.position = GetUpdatedPaddlePosition(relativePosX);
    }

    public Vector2 GetUpdatedPaddlePosition(float relativePosX)
    {
        // clamps the X position
        float clampedRelativePosX = Mathf.Clamp(relativePosX, minRelativePosX, maxRelativePosX);
        
        Vector2 newPaddlePosition = new Vector2(clampedRelativePosX, fixedRelativePosY);
        return newPaddlePosition;
    }
    
    public float ConvertPixelToRelativePosition(float pixelPosition, int screenWidth)
    { 
        var relativePosition = pixelPosition/screenWidth * screenWidthUnits;
        return relativePosition;
    }

}