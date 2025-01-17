using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using EnhancedUI.EnhancedScroller;

public class Level : SetupBehaviour
{
    [SerializeField] protected TextMeshProUGUI textLevel;
    public TextMeshProUGUI TextLevel => textLevel;
    [SerializeField] protected List<Image> stars;
    [SerializeField] protected Image spiderWeb;
    public Image SpiderWeb => spiderWeb;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetTextLevel();
        GetStars();
        GetSpiderWeb();
    }

    protected virtual void GetTextLevel()
    {
        if (textLevel != null) return;
        textLevel = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log("Reset " + nameof(textLevel) + " in " + GetType().Name);
    }
    protected virtual void GetStars()
    {
        if (stars.Count != 0) return;
        stars = new List<Image>(GetComponentsInChildren<Image>());
        stars.RemoveAt(0);
        stars.RemoveAt(stars.Count - 1);
        foreach (Image star in stars)
        {
            star.enabled = false;
        }
        Debug.Log("Reset " + nameof(stars) + " in " + GetType().Name);
    }
   
    protected virtual void GetSpiderWeb()
    {
        if (spiderWeb != null) return;
        spiderWeb = transform.Find("SpiderWeb").GetComponent<Image>();
        spiderWeb.enabled = false;
        Debug.Log("Reset " + nameof(spiderWeb) + " in " + GetType().Name);
    }
    public virtual void SetStar(int numberOfStar)
    {
        for (int i = 0; i < numberOfStar; i++)
        {
            stars[i].enabled = true;
        }
    }
}
