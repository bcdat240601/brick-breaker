using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelMapController : SetupBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;
    public float CellViewSize;
    protected virtual void Start()
    {
        scroller.Delegate = this;
    }
    protected override void LoadComponents()
    {
        GetScroller();
        GetCellViewPrefab();
    }

    protected virtual void GetCellViewPrefab()
    {
        if (cellViewPrefab != null) return;
        string prefabPath = "Assets/Prefabs/LevelMap/LevelGroup.prefab";
        cellViewPrefab = AssetDatabase.LoadAssetAtPath<EnhancedScrollerCellView>(prefabPath);
        Debug.Log("Reset " + nameof(cellViewPrefab) + " in " + GetType().Name);
    }


    protected virtual void GetScroller()
    {
        if (scroller != null) return;
        scroller = transform.parent.GetComponentInChildren<EnhancedScroller>();
        Debug.Log("Reset " + nameof(scroller) + " in " + GetType().Name);
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        LevelCellView cellView = scroller.GetCellView(cellViewPrefab) as LevelCellView;
        cellView.SetData(dataIndex, FindMaxNumber(), CellViewSize, FindNumberItemPerRow());
        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        CellViewSize = 330;
        return CellViewSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return FindMaxNumber();
    }
    protected virtual int FindMaxNumber()
    {
        int dataLength = ConfigManager.Instance.LevelDataSO.LevelDataList.Count;
        int remainder = dataLength % FindNumberItemPerRow();
        if (remainder == 0)
            return ConfigManager.Instance.LevelDataSO.LevelDataList.Count / FindNumberItemPerRow();
        else
            return ConfigManager.Instance.LevelDataSO.LevelDataList.Count / FindNumberItemPerRow() + 1;            
    }
    protected virtual int FindNumberItemPerRow()
    {
        LevelCellView levelCellView = (LevelCellView)cellViewPrefab;
        int numberItemPerRow = ((int)(Screen.width + levelCellView.LayoutGroup.spacing) / (int)(levelCellView.LevelPrefabs.rect.width + levelCellView.LayoutGroup.spacing));
        return numberItemPerRow;
    }

}
