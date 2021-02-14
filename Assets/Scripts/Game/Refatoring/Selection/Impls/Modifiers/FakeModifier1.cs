﻿using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class FakeModifier1 : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;

        public SelectionType Type { get => type; set => type = value; }

        public ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection, SelectionType type)
        {
            Debug.Log("FakeModifier1");
            return actualSelection;
        }
    }
}
