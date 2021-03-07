using System.Linq;
using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.RTSUserInterface.Utils;
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
        [SerializeField] private RectTransform _infoPanel;

        public GraphicRaycaster Raycaster { get => raycaster; set => raycaster = value; }
        public RectTransform ActionPanel { get => _actionPanel; set => _actionPanel = value; }
        public RectTransform BannerPanel { get => _bannerPanel; set => _bannerPanel = value; }
        public RectTransform ItemPanel { get => _itemPanel; set => _itemPanel = value; }
        public RectTransform MiniaturePanel { get => _miniaturePanel; set => _miniaturePanel = value; }
        public RectTransform PortraitPanel { get => _portraitPanel; set => _portraitPanel = value; }
        public RectTransform InfoPanel { get => _infoPanel; set => _infoPanel = value; }

        private void Awake()
        {
            UserInterfaceUtils.ClearPanel(PortraitPanel);
            UserInterfaceUtils.ClearPanel(ItemPanel);
            UserInterfaceUtils.ClearPanel(MiniaturePanel);
            UserInterfaceUtils.ClearPanel(BannerPanel);
            UserInterfaceUtils.ClearPanel(ActionPanel);
            UserInterfaceUtils.ClearPanel(InfoPanel);
        }

    }
}