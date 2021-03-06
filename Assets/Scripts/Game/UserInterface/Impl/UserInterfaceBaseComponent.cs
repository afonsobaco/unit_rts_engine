using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using UnityEngine.UI;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class UserInterfaceBaseComponent : MonoBehaviour
    {

        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private RectTransform _actionPanel;
        [SerializeField] private RectTransform _bannerPanel;
        [SerializeField] private RectTransform _itemPanel;
        [SerializeField] private RectTransform _miniaturePanel;
        [SerializeField] private RectTransform _portraitPanel;

        public RectTransform ActionPanel { get => _actionPanel; set => _actionPanel = value; }
        public RectTransform BannerPanel { get => _bannerPanel; set => _bannerPanel = value; }
        public RectTransform ItemPanel { get => _itemPanel; set => _itemPanel = value; }
        public RectTransform MiniaturePanel { get => _miniaturePanel; set => _miniaturePanel = value; }
        public RectTransform PortraitPanel { get => _portraitPanel; set => _portraitPanel = value; }
        public GraphicRaycaster Raycaster { get => raycaster; set => raycaster = value; }

        public void ClearPanel(RectTransform panel)
        {
            if (panel)
            {
                foreach (Transform child in panel)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        private void Start()
        {
            ClearPanel(PortraitPanel);
            ClearPanel(ItemPanel);
            ClearPanel(MiniaturePanel);
            ClearPanel(BannerPanel);
            ClearPanel(ActionPanel);
        }

    }
}