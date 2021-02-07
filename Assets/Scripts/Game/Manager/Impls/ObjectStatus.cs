using UnityEngine;
using System;

namespace RTSEngine.Manager
{
    [Serializable]
    public class ObjectStatus
    {
        [Space]
        [SerializeField] private int value = 1;
        [SerializeField] private int maxValue = 1;
        [SerializeField] private bool enabled = true;

        public int Value
        {
            get => value; set
            {
                if (value > maxValue)
                {
                    this.value = maxValue;
                }
                else
                {
                    this.value = value;
                }
            }
        }
        public int MaxValue
        {
            get
            {
                if (maxValue <= 0) return 1; else return maxValue;
            }
            set => maxValue = value;
        }
        public bool Enabled { get => enabled; set => enabled = value; }
    }



}
