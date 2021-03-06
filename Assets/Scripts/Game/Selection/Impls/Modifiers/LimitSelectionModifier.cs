using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSSelection
{

    [CreateAssetMenu(fileName = "LimitSelectionModifier", menuName = "Modifiers/LimitSelectionModifier")]
    public class LimitSelectionModifier : BaseSelectionModifier
    {
        [Space]
        [Header("Modifier attributes")]
        [SerializeField] [Range(1, 100)] private int _limit = 20;

        private Modifier _modifier;

        public override void StartVariables()
        {
            if (_modifier == null)
            {
                _modifier = new Modifier();
            }
            _modifier.Limit = _limit;
        }

        public override ISelectable[] Apply(SelectionInfo info)
        {
            StartVariables();
            return this._modifier.Apply(info.ActualSelection);
        }

        public class Modifier
        {
            public int Limit { get; set; }

            public ISelectable[] Apply(ISelectable[] actualSelection)
            {
                IEnumerable<ISelectable> result = actualSelection.ToList().Take(Limit);
                return result.ToArray();
            }
        }
    }

}
