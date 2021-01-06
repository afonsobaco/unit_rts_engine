using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SelectableObject : MonoBehaviour
{

    private bool _selected = false;
    public SelectionMark selectionMark;

    //TODO should be an Enum?
    public string typeStr;

    public bool IsSelected
    {
        get { return _selected; }
        set
        {
            selectionMark.transform.gameObject.SetActive(value);
            _selected = value;
        }
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