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
        private IEqualityComparer<ISelectable> _equalityComparer;

        public UserInterfaceManager(GameSignalBus signalBus, IEqualityComparer<ISelectable> equalityComparer)
        {
            _signalBus = signalBus;
            _equalityComparer = equalityComparer;
        }

        public void DoMiniatureClicked(ISelectable clicked)
        {
            if (clicked != null)
            {
                //TODO test this intergated
                Debug.Log(" Perform Selection On " + clicked.Position);
                _signalBus.Fire(new IndividualSelectionSignal() { Clicked = clicked, OnSelection = true });
            }
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

        public void DoBannerClicked(object partyId)
        {
            if (partyId != null)
            {
                //TODO test this intergated
                Debug.Log(" Get Party at " + partyId);
                _signalBus.Fire(new PartySelectionSignal() { CreateNew = false, PartyId = partyId });
            }
        }

        public void DoMapClicked(ISelectable selection)
        {
            if (selection != null)
            {
            }
        }

        public void DoActionClicked(ISelectable selection)
        {
            if (selection != null)
            {
            }
        }
    }
}
