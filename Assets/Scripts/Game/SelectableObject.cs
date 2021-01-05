using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SelectableObject : MonoBehaviour
{

    private bool _selected = false;
    public Transform selectionCircle;

    //TODO should be an Enum?
    public string typeStr;

    public bool IsSelected
    {
        get { return _selected; }
        set
        {
            var sr = selectionCircle.GetComponent<SpriteRenderer>();
            sr.enabled = value;
            sr.color = selectedColor;
            // outline.enabled = value;
            // outline.OutlineColor = selectedColor;
            _selected = value;
        }
    }
    public Color selectedColor;

    private Outline outline;
    void Start()
    {
        outline = this.GetComponent<Outline>();
        outline.enabled = false;
    }

    void OnEnable()
    {
        SelectionManager.Instance.AddToMainList(this);
    }

    void OnDisable()
    {
        SelectionManager.Instance.RemoveFromMainList(this);
    }

}