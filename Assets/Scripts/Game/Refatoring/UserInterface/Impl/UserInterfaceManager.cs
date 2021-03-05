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
        private UserInterface _userInterface;

        public UserInterfaceManager(GameSignalBus signalBus, UserInterface userInterface)
        {
            _signalBus = signalBus;
            _userInterface = userInterface;
        }

        public void DoMiniatureClicked(ISelectable clicked)
        {
            if (clicked != null)
            {
                //TODO test this intergated
                Debug.Log(" Perform Selection On " + clicked.Position);
                _signalBus.Fire(new IndividualSelectionSignal() { Clicked = clicked, IsUISelection = true });
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
