using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Script used to add vertical selection to a menu selector. It exposes the variable
 * SelectedMenuOptionIndex for subclasses to know the actual menu position.
 */
public class VerticalMenuSelector : SceneLoaderConnect
{
    // configuration
    [SerializeField] protected GameObject[] verticalMenuOptions;

    
    // state
    private int _selectedMenuOptionIndex = 0;  // (up, down) arrows

    /**
     * Returns the currently selected menu option as a Unity Game Object.
     */
    protected GameObject GetCurrentMenu()
    {
        return this.verticalMenuOptions[this._selectedMenuOptionIndex];
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        GetVericalMenuOption();
    }

    protected virtual void GetVericalMenuOption()
    {
        Transform menuOptionTransform = transform.parent.Find("MenuOptions");
        int childCount = menuOptionTransform.childCount;
        GameObject[] childObjects = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = menuOptionTransform.GetChild(i);
            childObjects[i] = childTransform.gameObject;
        }
        if (verticalMenuOptions.Length == childObjects.Length) return;
        verticalMenuOptions = childObjects;
        Debug.Log("Reset " + nameof(verticalMenuOptions) + " in " + GetType().Name);
    }

    /**
     * Returns a new Vector2 position for the menu selector position. The position is calculated
     * according to the currently selected menu option (private variable: this._selectedMenuOptionIndex).
     */
    protected Vector2 GetMenuSelectorPosition()
    {
        GameObject menuOption = this.verticalMenuOptions[this._selectedMenuOptionIndex];
        
        var selectorY = menuOption.transform.position.y;
        Vector2 selectorPosition = new Vector2(transform.position.x, selectorY);

        return selectorPosition;
    }
    
    /**
     * Handles up and down keyboard arrow presses by moving the menu selector position up and down
     * according to the menu available options (this.verticalMenuOptions).
     */
    protected void HandleUpDownArrowPresses()
    {
        var maxMenuOptionIndex = this.verticalMenuOptions.Length - 1;
        
        // handles (up, down) arrow presses + clamping for safety
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this._selectedMenuOptionIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this._selectedMenuOptionIndex--;
        }
        this._selectedMenuOptionIndex = Mathf.Clamp(this._selectedMenuOptionIndex, 0, maxMenuOptionIndex);
        
        // updates the menu selector position on the screen
        this.transform.position = GetMenuSelectorPosition();
    }
}
