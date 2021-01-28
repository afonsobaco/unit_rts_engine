using UnityEngine;
namespace RTSEngine.Manager
{

    public class BaseSelectionModSO : ScriptableObject, IBaseSelectionMod
    {


        [SerializeField] private SelectionTypeEnum type;
        [SerializeField] private bool active = true;
        [SerializeField] private bool activeOnPreSelection = true;

        public SelectionTypeEnum Type { get => type; set => type = value; }
        public bool Active { get => active; set => active = value; }
        public bool ActiveOnPreSelection { get => activeOnPreSelection; set => activeOnPreSelection = value; }
        public virtual ISelectionModifier SelectionModifier { get; set; }

        public virtual SelectionArgsXP Apply(SelectionArgsXP args)
        {
            return args;
        }

    }

}
