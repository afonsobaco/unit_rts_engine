using UnityEngine;
using System;

namespace RTSEngine.Manager
{
    public class ObjectStatus
    {
        private int currentValue;
        private int maxValue;
        private bool enabled = true;

        public int CurrentValue
        {
            get => currentValue;
            set
            {
                this.currentValue = Mathf.Clamp(value, 0, this.maxValue);
            }
        }
        public int MaxValue
        {
            get
            {
                if (this.maxValue <= 0) return 1; else return this.maxValue;
            }
            set
            {
                if (value == 0)
                {
                    this.enabled = false;
                    this.currentValue = 0;
                }
                else
                {
                    if (this.currentValue <= 0 || this.maxValue > value)
                    {
                        this.currentValue = value;
                    }
                    else
                    {
                        this.currentValue = (int)(value * (this.currentValue / this.maxValue));
                    }
                }
                this.maxValue = value;

            }
        }
        public bool Enabled { get => this.enabled; set => this.enabled = value; }
    }



}
