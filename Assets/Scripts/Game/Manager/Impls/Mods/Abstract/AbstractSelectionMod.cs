using UnityEngine;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public abstract class AbstractSelectionMod<T, ST> : MonoBehaviour, IAbstractSelectionMod<T, ST>
    {

        [SerializeField] private bool active = true;
        private ST type;

        public bool Active { get => active; set => active = value; }
        public ST Type { get => type; set => type = value; }

        public abstract SelectionArgsXP<T, ST> Apply(SelectionArgsXP<T, ST> args);


    }
}
