using System.Collections.Generic;
using System.Linq;
using RTSEngine.Signal;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceSignalManager
    {
        private UserInterfaceBase _userInterfaceBase;
        private UserInterfaceManager _userInterfaceManager;
        private UserInterface _userInterface;

        public UserInterfaceSignalManager(UserInterfaceManager userInterfaceManager, UserInterface userInterface, UserInterfaceBase userInterfaceBase)
        {
            this._userInterfaceManager = userInterfaceManager;
            this._userInterface = userInterface;
            this._userInterfaceBase = userInterfaceBase;
        }

        public void OnSelectionUpdate(SelectionUpdateSignal signal)
        {
            _userInterface.DoSelectionUpdate(signal.Selection);
            _userInterfaceBase.UpdateAll();
        }

        public void OnPartyUpdate(PartyUpdateSignal signal)
        {
            _userInterface.DoPartyUpdate(signal.Parties);
            _userInterfaceBase.UpdateBanners();
        }

        public void OnAlternateSubGroup(AlternateSubGroupSignal signal)
        {
            _userInterface.AlternateSubGroup(signal.Previous);
            _userInterfaceBase.UpdateAll();
        }
        public void OnMiniatureClicked(MiniatureClickedSignal signal)
        {
            _userInterfaceManager.DoMiniatureClicked(signal.Selected);
        }
        public void OnPortraitClicked(PortraitClickedSignal signal)
        {
            _userInterfaceManager.DoPortraitClicked(signal.Selection);
        }
        public void OnBannerClicked(BannerClickedSignal signal)
        {
            Core.ISelectable[] value;
            _userInterface.Parties.TryGetValue(signal.PartyId, out value);
            _userInterfaceManager.DoBannerClicked(value);
        }
        public void OnMapClicked(MapClickedSignal signal)
        {
            _userInterfaceManager.DoMapClicked(signal.Selection);
        }
        public void OnActionClicked(ActionClickedSignal signal)
        {
            _userInterfaceManager.DoActionClicked(signal.Selection);
        }

    }
}
