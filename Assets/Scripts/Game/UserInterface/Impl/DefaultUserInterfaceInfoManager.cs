using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        [Inject] private UserInterfaceBase userInterfaceBase;

        private List<DefaultInfoButton> _beingRemoved = new List<DefaultInfoButton>();
        private List<DefaultInfoButton> _beingAdded = new List<DefaultInfoButton>();

        public RectTransform InfoPanel { get => userInterfaceBase.UserInterfaceBaseComponent.InfoPanel; }

        public void AddInfo(DefaultInfoButton button)
        {
            AddSingleInfo(button);
        }

        public void RemoveInfo(DefaultInfoButton button)
        {
            RemoveSingleInfo(button);
        }

        public void AddAllInfo(DefaultInfoButton[] buttons)
        {
            foreach (var button in buttons)
            {
                AddSingleInfo(button);
            }
        }

        public void RemoveAllInfo(DefaultInfoButton[] buttons)
        {
            foreach (var button in buttons)
            {
                RemoveSingleInfo(button);
            }
        }

        public void Clear()
        {
            StartCoroutine(ClearPanel());
        }

        private IEnumerator ClearPanel()
        {
            foreach (var button in InfoPanel.GetComponentsInChildren<DefaultInfoButton>(true))
            {
                StartCoroutine(DoRemove(button));
            }
            yield return null;
        }

        private void AddSingleInfo(DefaultInfoButton button)
        {
            if (!PanelContainsInfo(GetTextOn(button)))
            {
                button.gameObject.SetActive(false);
                button.transform.SetParent(InfoPanel.transform, false);
                UpdateInfoPanel();
            }
            else
            {
                Destroy(button);
            }
        }

        private void RemoveSingleInfo(DefaultInfoButton button)
        {
            if (PanelContainsInfo(GetTextOn(button)) && !_beingRemoved.Contains(button))
            {
                _beingRemoved.Add(button);
                StartCoroutine(DoRemove(button));
            }
            else
            {
                Destroy(button);
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
            var activeChildren = InfoPanel.GetComponentsInChildren<DefaultInfoButton>(false).ToList();
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
            return GameUtils.GetAllInactiveChildren<DefaultInfoButton>(InfoPanel.gameObject).Take(diff).ToArray();
        }

        public virtual IEnumerator DoDestroyAnim(DefaultInfoButton button)
        {
            yield return StartCoroutine(button.DoDestroyAnim());
        }

        public virtual IEnumerator DoCreateAnim(DefaultInfoButton button)
        {
            yield return StartCoroutine(button.DoCreateAnim());
        }

        public bool PanelContainsInfo(string info)
        {
            foreach (var item in InfoPanel.GetComponentsInChildren<DefaultInfoButton>(true))
            {
                if (GetTextOn(item).Equals(info))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetTextOn(DefaultInfoButton button)
        {
            return GameUtils.FindInComponent<Text>(button.Text.gameObject).text;
        }


    }

}