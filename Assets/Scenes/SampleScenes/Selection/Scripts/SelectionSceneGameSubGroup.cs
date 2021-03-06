using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.RTSSelection.Scene
{

    public class SelectionSceneGameSubGroup : MonoBehaviour, IComparable
    {
        [SerializeField] private string subGroup;
        [SerializeField] private int priority;

        public string SubGroup { get => subGroup; set => subGroup = value; }
        public int Priority { get => priority; set => priority = value; }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return -1;
            }
            var other = obj as SelectionSceneGameSubGroup;
            //Compare Statuses
            return this.Priority - other.Priority;
        }
    }

}