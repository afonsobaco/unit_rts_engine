using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;
using RTSEngine.Signal;

namespace RTSEngine.Integration.Scene
{
    public class UIPartyContainerManager : UILimitedContainerManager
    {
        private void Start()
        {
            signalBus.Subscribe<UIUpdatePartySignal>(PartyUpdateReceiver);
            signalBus.Subscribe<UIPartySelectionSignal>(PartySelectionReceiver);
        }

        private void PartySelectionReceiver(UIPartySelectionSignal signal)
        {
            var party = FindPartyByKey(signal.PartyId);
            if (party)
            {
                signalBus.Fire(new ChangeSelectionSignal() { Selection = (party.Info as UIPartyContentInfo).Selection });
            }
        }

        private void PartyUpdateReceiver(UIUpdatePartySignal signal)
        {
            if (signal.Selection.Length > 0)
            {
                UpdateParty(signal.Selection, signal.PartyId);
            }
            else
            {
                RemoveParty(signal.PartyId);
            }
        }

        private void UpdateParty(IntegrationSceneObject[] selection, int key)
        {
            var party = FindPartyByKey(key);
            UIPartyContentInfo partyInfo = new UIPartyContentInfo() { Key = key, Selection = selection };
            if (party)
            {
                party.Info = partyInfo;
                StartCoroutine(base.UpdateContainerRoutine(new UIContainerInfo() { ContainerId = container.ContainerId }, true, true));
            }
            else
            {
                StartCoroutine(base.AddToContainerRoutine(partyInfo, true, true));
            }
        }

        private void RemoveParty(int key)
        {
            var party = FindPartyByKey(key);
            if (party)
                StartCoroutine(base.RemoveFromContainerRoutine(party, true, true));
        }

        public override UIContent AddToContainer(UIContentInfo contentInfo)
        {
            var content = base.AddToContainer(contentInfo);
            if (content)
            {
                SortPartiesByKey();
            }
            return content;
        }

        private UIContent FindPartyByKey(int key)
        {
            return GetUIContentChildren().Find(x => (x.Info as UIPartyContentInfo).Key == key);
        }

        private void SortPartiesByKey()
        {
            List<UIContent> items = GetUIContentChildren();
            items.Sort(new PartyComparer());
            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.SetSiblingIndex(i);
            }
        }
    }

    internal class PartyComparer : IComparer<UIContent>
    {
        public int Compare(UIContent x, UIContent y)
        {
            var xKey = (x.Info as UIPartyContentInfo).Key;
            var yKey = (y.Info as UIPartyContentInfo).Key;
            return xKey - yKey;
        }
    }
}