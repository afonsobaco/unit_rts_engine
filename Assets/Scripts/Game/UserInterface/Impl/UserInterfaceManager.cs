using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class UserInterfaceManager
    {
        private SignalBus _signalBus;
        private UserInterface _userInterface;

        public UserInterfaceManager(SignalBus signalBus, UserInterface userInterface)
        {
            _signalBus = signalBus;
            _userInterface = userInterface;
        }

        public void DoMiniatureClicked(ISelectable clicked)
        {
            if (clicked != null)
            {
                _signalBus.Fire(new IndividualSelectionSignal() { Clicked = clicked, IsUISelection = true });
            }
        }

        public void DoPortraitClicked(ISelectable clicked)
        {
            if (clicked != null)
            {
                _signalBus.Fire(new CameraGoToPositionSignal() { Position = clicked.Position });
            }
        }

        public void DoBannerClicked(object partyId)
        {
            if (partyId != null)
            {
                _signalBus.Fire(new ChangeSelectionSignal() { Selection = _userInterface.GetParty(partyId) });
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
