using System;
using UnityEngine;

namespace RTSEngine.Integration.Scene
{

    public class IntegrationSceneGameSubGroup : MonoBehaviour, IComparable
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
            var other = obj as IntegrationSceneGameSubGroup;
            //Compare Statuses
            return this.priority - other.priority;
        }
    }

}