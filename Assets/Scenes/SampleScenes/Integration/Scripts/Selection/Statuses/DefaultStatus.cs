using UnityEngine;

namespace RTSEngine.Integration.Scene
{
    public abstract class DefaultStatus : MonoBehaviour
    {
        [SerializeField] private int _maxValue = 20;
        [SerializeField] private int _value = 20;

        public int MaxValue { get => _maxValue; set => _maxValue = value; }
        public int Value { get => _value; set => _value = value; }
    }

}