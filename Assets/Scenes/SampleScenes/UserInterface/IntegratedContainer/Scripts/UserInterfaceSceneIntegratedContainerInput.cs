using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneIntegratedContainerInput : MonoBehaviour
    {

        [Inject] private GameSignalBus _signalBus;
        [SerializeField] private UIMiniatureSelectionManager uIMiniatureSelectionManager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = "LogContainer" }, Info = new UIContentInfo() { } });
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = "BannerContainer" }, Info = new UIBannerContentInfo() { Key = Random.Range(1, 11) } });
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //TODO rename to notification
                _signalBus.Fire(new UIAddContentSignal { ContainerInfo = new UIContainerInfo() { ContainerId = "InfoContainer" }, Info = GetInfo() });
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = "ProfileContainer" }, Info = new UIContentInfo() { } });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetRandomSelectionMiniature();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _signalBus.Fire(new UIClearContainerSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = "MiniatureContainer" } });
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = "ItemContainer" }, Info = new UIContentInfo() { } });
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = "ActionContainer" }, Info = new UIContentInfo() { } });
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeHighlighted();
            }
        }

        private void GetRandomSelectionMiniature()
        {
            List<UIContentInfo> infoList = uIMiniatureSelectionManager.GetRandomSelection().Cast<UIContentInfo>().ToList();
            UIContainerInfo miniatureContainerInfo = new UIMiniatureContainerInfo() { ContainerId = "MiniatureContainer" };
            _signalBus.Fire(new UIAddAllContentSignal() { ContainerInfo = miniatureContainerInfo, InfoList = infoList });
        }

        private void ChangeHighlighted()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                UIMiniatureContainerInfo previousInfo = new UIMiniatureContainerInfo() { OldSelection = true, NextHighlight = false, ContainerId = "MiniatureContainer" };
                _signalBus.Fire(new UIUpdateContainerSignal() { ContainerInfo = previousInfo });
            }
            else
            {
                UIMiniatureContainerInfo nextInfo = new UIMiniatureContainerInfo() { OldSelection = true, NextHighlight = true, ContainerId = "MiniatureContainer" };
                _signalBus.Fire(new UIUpdateContainerSignal() { ContainerInfo = nextInfo });
            }
        }

        private static UIInfoContentInfo GetInfo()
        {
            return new UIInfoContentInfo
            {
                MainText = " Main Text",
                Tooltip = " ToolTip Text",
                SubText = " Click to Dismiss",
                Title = " Title text",
            };
        }


    }
}

