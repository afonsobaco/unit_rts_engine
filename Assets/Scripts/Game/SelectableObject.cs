using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectableObject : MonoBehaviour
{

    private bool _selected = false;
    public Transform selectionCircle;

    public bool Selected
    {
        get { return _selected; }
        set
        {
            var sr= selectionCircle.GetComponent<SpriteRenderer>();
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