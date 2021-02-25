using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceManager
    {
        private GameSignalBus _signalBus;

        public UserInterfaceManager(GameSignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void DoMiniatureClicked(ISelectable[] selection, ISelectable clicked, bool toRemove, bool asSubGroup)
        {
            List<ISelectable> newSelection = new List<ISelectable>(selection);
            List<ISelectable> subGroup = new List<ISelectable>();

            if (asSubGroup)
                subGroup = GetAllFromSameSubGroup(selection, clicked);
            else
                subGroup.Add(clicked);

            if (toRemove)
                newSelection.RemoveAll(x => subGroup.Contains(x));
            else
                newSelection = subGroup;

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

        public void DoPortraitClicked(ISelectable clicked)
        {
            if (clicked != null)
            {
                //TODO test this intergated
                Debug.Log("camera goes to position " + clicked.Position);
                _signalBus.Fire(new CameraGoToPositionSignal() { Position = clicked.Position });
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
        }

        public void DoActionClicked(ISelectable selection)
        {
        }
    }
}
