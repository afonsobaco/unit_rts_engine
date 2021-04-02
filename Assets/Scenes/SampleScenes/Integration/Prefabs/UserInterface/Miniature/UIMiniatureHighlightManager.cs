using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RTSEngine.Core;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{

    public class UIMiniatureHighlightManager
    {
        private IEqualityComparer<ISelectable> _equalityComparer;
        public UIMiniatureHighlightManager(IEqualityComparer<ISelectable> equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }
        private IntegrationSceneObject _highlighted;
        public IntegrationSceneObject Highlighted { get => _highlighted; set => _highlighted = value; }

        public void UpdateMiniaturesHighlight(List<IntegrationSceneObject> selectables, bool isUISelection)
        {
            SetHighlighted(selectables, isUISelection);
            if (_highlighted != null)
            {
                selectables.ForEach(x => x.IsHighlighted = _equalityComparer.Equals(_highlighted, x));
            }
        }

        private void SetHighlighted(List<IntegrationSceneObject> selectables, bool isUISelection)
        {
            if (selectables.Count > 0)
            {
                if (!isUISelection || _highlighted == null || !selectables.Any(x => _equalityComparer.Equals(_highlighted, x)))
                    _highlighted = selectables[0];
            }
            else
            {
                _highlighted = null;
            }
        }

        public void UpdateHighlighted(UIContainerInfo containerInfo, List<UIContent> contentList)
        {
            var info = containerInfo as UIMiniatureContainerInfo;
            if (contentList.Count > 0)
            {
                if (!info.OldSelection || _highlighted == null)
                    _highlighted = UIUtils.GetSelectable(contentList[0].Info);
                else
                {
                    List<IntegrationSceneObject> selection = UIUtils.GetSelectableFromContent(contentList);
                    if (info.NextHighlight)
                        _highlighted = GetNextHighlight(selection);
                    else
                        _highlighted = GetPreviousHighlight(selection);
                }
            }
            else
            {
                _highlighted = null;
            }
        }

        public IntegrationSceneObject GetPreviousHighlight(List<IntegrationSceneObject> selection)
        {
            var result = selection.Last();
            int index = selection.IndexOf(_highlighted);
            if (index > 0)
            {
                result = selection[index - 1];
            }
            return selection.Find(x => _equalityComparer.Equals(result, x));
        }

        public IntegrationSceneObject GetNextHighlight(List<IntegrationSceneObject> selection)
        {
            var result = selection[0];
            int index = selection.IndexOf(_highlighted);
            if (index < selection.Count - 1)
            {
                var next = selection.GetRange(index, selection.Count - index).Find(x => !_equalityComparer.Equals(_highlighted, x));
                if (next != null)
                {
                    result = next;
                }
            }
            return result;
        }
    }

}