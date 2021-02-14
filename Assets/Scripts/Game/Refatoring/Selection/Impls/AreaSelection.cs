
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

        public ISelectable[] GetSelection(ISelectable[] mainList, Vector2 startPoint, Vector2 endPoint)
        {
            List<ISelectable> result = new List<ISelectable>();
            if (mainList == null)
            {
                return result.ToArray();
            }
            for (int i = 0; i < mainList.Length; i++)
            {
                if (_areaSelection.IsInsideSelectionArea(startPoint, endPoint, mainList[i]))
                {
                    result.Add(mainList[i]);
                }
            }
            return DistanceHelper.Sort(result, startPoint);
        }
    }
}
