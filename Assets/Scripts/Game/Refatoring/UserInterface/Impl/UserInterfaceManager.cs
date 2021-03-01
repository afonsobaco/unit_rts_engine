using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceManager
    {
        private GameSignalBus _signalBus;
        private EqualityComparerComponent _equalityComparer;

        public UserInterfaceManager(GameSignalBus signalBus, EqualityComparerComponent equalityComparer)
        {
            _signalBus = signalBus;
            _equalityComparer = equalityComparer;
        }

        public void DoMiniatureClicked(ISelectable clicked)
        {
            if (clicked != null)
            {
                _signalBus.Fire(new ChangeSelectionSignal() { Selection = new ISelectable[] { clicked } });
            }
        }

        private List<ISelectable> GetAllFromSameSubGroup(ISelectable[] selection, ISelectable selected)
        {
            List<ISelectable> selectables = selection.ToList().FindAll(x =>
            {
                return _equalityComparer.Equals(x, selected);
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

        public void DoBannerClicked(ISelectable[] group)
        {
            if (group != null)
            {
                _signalBus.Fire(new SelectionUpdateSignal() { Selection = group });
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
