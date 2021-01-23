using UnityEngine;

namespace RTSEngine.Manager.SelectionMods
{
    public abstract class AbstractSelectionMod<T, ST> : ScriptableObject, ISelectionMod<T, ST>
    {

        [SerializeField] private bool active = true;
        [SerializeField] private ST type;

        public bool Active { get => active; set => active = value; }
        public ST Type { get => type; set => type = value; }

        public abstract ISelectionArgsXP<T, ST> Apply(ISelectionArgsXP<T, ST> args);
    }
}
