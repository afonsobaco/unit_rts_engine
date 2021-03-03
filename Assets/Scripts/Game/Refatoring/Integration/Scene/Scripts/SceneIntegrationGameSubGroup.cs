using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class SceneIntegrationGameSubGroup : MonoBehaviour, IComparable
    {
        [SerializeField] private string subGroup;
        [SerializeField] private int priority;

        public string SubGroup { get => subGroup; }

        public int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return -1;
            }
            var other = obj as SceneIntegrationGameSubGroup;
            //Compare Statuses
            return this.priority - other.priority;
        }
    }

}