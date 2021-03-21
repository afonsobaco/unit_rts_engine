using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UIMiniatureContentInfo : UIContentInfo
    {
        private float _health;
        private float _mana;
        private float _maxHealth;
        private float _maxMana;
        private Sprite _picture;
        private bool _highlighted;

        private UIMiniatureSelectable _selectable;

        public float Health { get => _health; set => _health = value; }
        public float Mana { get => _mana; set => _mana = value; }
        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public float MaxMana { get => _maxMana; set => _maxMana = value; }
        public Sprite Picture { get => _picture; set => _picture = value; }
        public UIMiniatureSelectable Selectable { get => _selectable; set => _selectable = value; }
        public bool Highlighted { get => _highlighted; set => _highlighted = value; }
    }
}