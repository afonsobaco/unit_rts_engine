using UnityEngine;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public abstract class AbstractSelectionMod<T, E> : MonoBehaviour, IAbstractSelectionMod<T, E>
    {

        [SerializeField] private bool active = true;
        private E type;

        public bool Active { get => active; set => active = value; }
        public E Type { get => type; set => type = value; }

        public abstract SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args);


    }
}
