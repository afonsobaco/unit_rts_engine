using System.Linq;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
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

        public override ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection)
        {
            return this._modifier.Apply(actualSelection);
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
