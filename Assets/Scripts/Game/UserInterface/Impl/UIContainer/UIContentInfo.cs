using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContentInfo
    {
        private UIContent _content;
        private bool _isBeeingAdded;
        private bool _isBeeingRemoved;
        public UIContent Content { get => _content; set => _content = value; }
        public bool IsBeeingAdded { get => _isBeeingAdded; set => _isBeeingAdded = value; }
        public bool IsBeeingRemoved { get => _isBeeingRemoved; set => _isBeeingRemoved = value; }
    }
}