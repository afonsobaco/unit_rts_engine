using UnityEngine;
namespace RTSEngine.Manager
{

    public class BaseSelectionModSO : ScriptableObject, IBaseSelectionMod
    {

        private IBaseSelectionMod modifier;
        [SerializeField] private SelectionTypeEnum type;
        [SerializeField] private bool active = true;
        [SerializeField] private bool activeOnPreSelection = true;


        protected IBaseSelectionMod Modifier { get => modifier; set => modifier = value; }
        public SelectionTypeEnum Type { get => type; set => type = value; }
        public bool Active { get => active; set => active = value; }
        public bool ActiveOnPreSelection { get => activeOnPreSelection; set => activeOnPreSelection = value; }

        public virtual SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return args;
        }

    }

}
