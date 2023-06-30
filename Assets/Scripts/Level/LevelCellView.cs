using EnhancedUI.EnhancedScroller;
using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelCellView : EnhancedScrollerCellView
{
    [SerializeField] protected int levelGroupIndex;
    [SerializeField] protected List<TextMeshProUGUI> textList;
    [SerializeField] protected RectTransform levelPrefabs;
    public RectTransform LevelPrefabs => levelPrefabs;
    [SerializeField] protected RectTransform HRobe;
    [SerializeField] protected RectTransform VRobe;
    [SerializeField] protected HorizontalLayoutGroup layoutGroup;
    public HorizontalLayoutGroup LayoutGroup => layoutGroup;
    [SerializeField] protected int maxNumberRow;
    [SerializeField] protected int quotient;
    [SerializeField] protected int remainder;
    [SerializeField] protected float cellViewSize;
    [SerializeField] protected RectTransform lastChild;
    [SerializeField] protected RectTransform firstChild;
    [SerializeField] protected int numberPerRow;
    public static bool canSetData = true;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetHorizontalLayout();
        GetLevelPrefabs();
        GetHRobe();
        GetVRobe();
    }

    private void GetVRobe()
    {
        if (VRobe != null) return;
        string prefabPath = "Assets/Prefabs/LevelMap/VRobe.prefab";
        VRobe = AssetDatabase.LoadAssetAtPath<RectTransform>(prefabPath);
        Debug.Log("Reset " + nameof(VRobe) + " in " + GetType().Name);
    }

    private void GetHRobe()
    {
        if (HRobe != null) return;
        string prefabPath = "Assets/Prefabs/LevelMap/HRobe.prefab";
        HRobe = AssetDatabase.LoadAssetAtPath<RectTransform>(prefabPath);
        Debug.Log("Reset " + nameof(HRobe) + " in " + GetType().Name);
    }

    protected virtual void GetLevelPrefabs()
    {
        if (levelPrefabs != null) return;
        string prefabPath = "Assets/Prefabs/LevelMap/Level.prefab";
        levelPrefabs = AssetDatabase.LoadAssetAtPath<RectTransform>(prefabPath);
        Debug.Log("Reset " + nameof(levelPrefabs) + " in " + GetType().Name);
    }

    protected virtual void GetHorizontalLayout()
    {
        if (layoutGroup != null) return;
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        Debug.Log("Reset " + nameof(layoutGroup) + " in " + GetType().Name);
    }

    public void SetData(int levelgroupIndex, int maxNumberRow, float cellViewSize, int numberPerRow)
    {
        if (!gameObject.activeSelf) return;
        this.levelGroupIndex = levelgroupIndex + 1;
        this.maxNumberRow = maxNumberRow;
        cellIdentifier = this.levelGroupIndex.ToString();
        this.cellViewSize = cellViewSize;
        this.numberPerRow = numberPerRow;
        //LevelData = ConfigManager.Instance.LevelDataSO.LevelDataList[levelIndex];
        //levelText.text = LevelData.LevelNumber.ToString();
        SetText();        
    }
    protected virtual void SetText()
    {
        
        int dataLength = ConfigManager.Instance.LevelDataSO.LevelDataList.Count;
        if (levelGroupIndex == maxNumberRow)
            canSetData = false;
        AlignElementInGroup();
        quotient = dataLength / numberPerRow;
        remainder = dataLength % numberPerRow;
        int maxIndex;
        int minIndex;
        int t;
        if (quotient >= levelGroupIndex)
        {
            maxIndex = levelGroupIndex * numberPerRow;
            minIndex = maxIndex - numberPerRow + 1;
            t = minIndex;
            StartCoroutine("SpawnAndGetComponent", t);
        }
        else
        {
            minIndex = dataLength - remainder + 1;
            t = minIndex;
            StartCoroutine("SpawnAndGetComponent",t);
        }
            
    }
    protected virtual void AlignElementInGroup()
    {
        if (levelGroupIndex % 2 == 0)
        {
            layoutGroup.reverseArrangement = true;            
            layoutGroup.childAlignment = TextAnchor.MiddleRight;
        }
        else
        {
            layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        }
        layoutGroup.spacing = 63;
    }
    protected virtual bool SpawnLevel()
    {        
        if(quotient >= levelGroupIndex)
        {
            for (int i = 0; i < numberPerRow; i++)
            {
                Instantiate(levelPrefabs, transform);
            }
        }
        else
        {
            for (int i = 0; i < remainder; i++)
            {
                Instantiate(levelPrefabs, transform);
            }
        }        
        return true;
    }
    protected virtual void SpawnRobe()
    {
        lastChild = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
        firstChild = transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform Hrobe = Instantiate(HRobe, transform);


        Hrobe.SetAsFirstSibling();
        float HRobeWidth = lastChild.anchoredPosition.x - firstChild.anchoredPosition.x;
        HRobeWidth = Math.Abs(HRobeWidth);
        float HRobeX = (lastChild.anchoredPosition.x + firstChild.anchoredPosition.x) / 2;
        Hrobe.anchoredPosition = new Vector3(HRobeX, lastChild.anchoredPosition.y, 0);
        Hrobe.sizeDelta = new Vector2(HRobeWidth, Hrobe.sizeDelta.y);


        if (levelGroupIndex == maxNumberRow) return;
        RectTransform Vrobe = Instantiate(VRobe, transform);
        Vrobe.SetAsFirstSibling();
        Vrobe.anchoredPosition = new Vector3(lastChild.anchoredPosition.x, lastChild.anchoredPosition.y - cellViewSize / 2, 0);
    }


    IEnumerator SpawnAndGetComponent(int minIndex)
    {
        yield return new WaitUntil(SpawnLevel);
        SpawnRobe();
        foreach (Transform child in transform)
        {
            Level level;
            child.TryGetComponent<Level>(out level);
            if (level == null)
                continue;
            level.currentLevel = minIndex;
            level.SubcribeButton();
            level.TextLevel.text = minIndex.ToString();
            if (minIndex > ConfigManager.Instance.LevelDataSO.currentLevelHasPlayed + 1)
                level.SpiderWeb.enabled = true;
            level.SetStar(ConfigManager.Instance.LevelDataSO.LevelDataList[minIndex - 1].Star);
            minIndex++;
        }
    }
}
