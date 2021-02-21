using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceManager
    {

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void DoMiniatureClicked(ISelectable[] selection, ISelectable clicked, bool toRemove, bool asGroup)
        {
            List<ISelectable> newSelection = new List<ISelectable>(selection);
            List<ISelectable> group = new List<ISelectable>();

            if (asGroup)
            {
                group = GetAllFromSameSubGroup(selection, clicked);
            }
            else
            {
                group.Add(clicked);
            }
            if (toRemove)
            {
                newSelection.RemoveAll(x => group.Contains(x));
            }
            else
            {
                newSelection = group;
            }
            _signalBus.Fire(new ChangeSelectionSignal() { Selection = newSelection.ToArray() });
        }

        private static List<ISelectable> GetAllFromSameSubGroup(ISelectable[] selection, ISelectable selected)
        {
            List<ISelectable> selectables = selection.ToList().FindAll(x =>
            {
                if (x is IGroupable)
                {
                    var item = x as IGroupable;
                    return item.IsCompatible(selected);
                }
                return false;
            });
            return selectables;
        }

        public void DoPortraitClicked(ISelectable selection)
        {
            if (selection != null)
            {
                //TODO test this intergated
                Debug.Log("camera goes to position " + selection.Position);
                _signalBus.Fire(new CameraGoToPositionSignal() { Position = selection.Position });
            }
        }

        public void DoBannerClicked(ISelectable[] selection, ISelectable[] group, bool toRemove)
        {
            if (group != null)
            {

                List<ISelectable> newSelection = new List<ISelectable>(selection);
                if (toRemove)
                {
                    if (group.ToList().TrueForAll(x => newSelection.Contains(x)))
                    {
                        newSelection.RemoveAll(x => group.Contains(x));
                    }
                    else
                    {
                        newSelection = newSelection.Union(group).ToList();
                    }
                }
                else
                {
                    newSelection = new List<ISelectable>(group);
                }
                _signalBus.Fire(new ChangeSelectionSignal() { Selection = newSelection.ToArray() });
            }
        }
        public void DoMapClicked(ISelectable selection)
        {
            //_signalBus.Fire(new MapClickedSignal() { });
        }

        public void DoActionClicked(ISelectable selection)
        {
            //_signalBus.Fire(new ActionClickedSignal() { });
        }
    }
}
