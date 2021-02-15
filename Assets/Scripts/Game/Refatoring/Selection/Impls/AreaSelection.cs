
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class AreaSelection : IAreaSelection
    {
        private IAreaSelectionType _areaSelection;

        public AreaSelection(IAreaSelectionType areaSelection)
        {
            this._areaSelection = areaSelection;
        }

        public ISelectable[] GetSelection(ISelectable[] mainList, Vector2 startScreenPoint, Vector2 endPoint)
        {
            List<ISelectable> result = new List<ISelectable>();
            if (mainList == null)
            {
                return result.ToArray();
            }
            for (int i = 0; i < mainList.Length; i++)
            {
                if (_areaSelection.IsInsideScreenPoints(startScreenPoint, endPoint, mainList[i]))
                {
                    result.Add(mainList[i]);
                }
            }
            ISelectable[] selectables = DistanceHelper.SortScreenSpace(result, startScreenPoint);
            return selectables;
        }
    }
}
