using UnityEngine;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls.Deprecated
{
    public abstract class AbstractSelectionMod<T, S, O> : MonoBehaviour, IAbstractSelectionMod<T, S, O>
    {

        [SerializeField] private bool active = true;
        private S type;

        public bool Active { get => active; set => active = value; }
        public S Type { get => type; set => type = value; }

        public abstract SelectionArgsXP<T, S, O> Apply(SelectionArgsXP<T, S, O> args);


    }
}
