using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;
using System.Linq;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultUserInterfaceInfoManager : MonoBehaviour, IInfoMessageTarget
    {
        [SerializeField] private int _maximunInfoOnScreen = 5;

        [SerializeField] private RectTransform _infoPanel;

        private List<DefaultInfoButton> _beingRemoved = new List<DefaultInfoButton>();

        private List<DefaultInfoButton> _beingAdded = new List<DefaultInfoButton>();

        public void AddInfo(DefaultInfoButton button)
        {
            if (!PanelContainsInfo(button.Text.text))
            {
                button.transform.SetParent(_infoPanel.transform, false);
                UpdateInfoPanel();
            }
        }

        public void RemoveInfo(DefaultInfoButton button)
        {
            if (PanelContainsInfo(button.Text.text) && !_beingRemoved.Contains(button))
            {
                _beingRemoved.Add(button);
                StartCoroutine(DoRemove(button));
            }
        }

        private IEnumerator DoRemove(DefaultInfoButton button)
        {
            yield return StartCoroutine(DoDestroyAnim(button));
            _beingRemoved.Remove(button);
            Destroy(button.gameObject);
            yield return new WaitForEndOfFrame();
            UpdateInfoPanel();
        }

        private IEnumerator DoAdd(DefaultInfoButton button)
        {
            yield return StartCoroutine(DoCreateAnim(button));
        }

        public virtual void UpdateInfoPanel()
        {
            var activeChildren = _infoPanel.GetComponentsInChildren<DefaultInfoButton>(false).ToList();
            activeChildren.RemoveAll(x => _beingRemoved.Contains(x));
            if (activeChildren.Count < _maximunInfoOnScreen)
            {
                DefaultInfoButton[] toBeAdded = GetToBeAdded(_maximunInfoOnScreen - activeChildren.Count);
                foreach (var item in toBeAdded)
                {
                    item.gameObject.SetActive(true);
                    StartCoroutine(DoAdd(item));
                }
            }
        }

        private DefaultInfoButton[] GetToBeAdded(int diff)
        {
            return GameUtils.GetAllInactiveChildren<DefaultInfoButton>(_infoPanel.gameObject).Take(diff).ToArray();
        }

        public virtual IEnumerator DoDestroyAnim(DefaultInfoButton button)
        {
            yield return null;
        }

        public virtual IEnumerator DoCreateAnim(DefaultInfoButton button)
        {
            yield return null;
        }

        public bool PanelContainsInfo(string info)
        {
            foreach (var item in _infoPanel.GetComponentsInChildren<DefaultInfoButton>(true))
            {
                if (item.Text.text.Equals(info))
                {
                    return true;
                }
            }
            return false;
        }
    }

}