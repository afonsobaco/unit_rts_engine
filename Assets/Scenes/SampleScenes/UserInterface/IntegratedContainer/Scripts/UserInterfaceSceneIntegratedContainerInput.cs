using UnityEngine;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneIntegratedContainerInput : MonoBehaviour
    {
        [SerializeField] private Sprite[] miniatures;

        [Inject] private GameSignalBus _signalBus;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = GetLogInfo() });
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = GetBannerInfo() });
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //TODO rename to notification
                _signalBus.Fire(new UIAddContentSignal { Info = GetInfo() });
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = GetProfileInfo() });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = GetRandomMiniatureInfo() });
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = GetItemInfo() });
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = GetActionInfo() });
            }
        }

        private static UIContentInfo GetLogInfo()
        {
            return new UIContentInfo
            {
                ContainerId = "LogContainer"
            };
        }

        private static UIContentInfo GetActionInfo()
        {
            return new UIContentInfo
            {
                ContainerId = "ActionContainer"
            };
        }

        private static UIContentInfo GetItemInfo()
        {
            return new UIContentInfo
            {
                ContainerId = "ItemContainer"
            };
        }

        private UIContentInfo GetProfileInfo()
        {
            throw new System.NotImplementedException();
        }

        private static UIBannerContentInfo GetBannerInfo()
        {
            return new UIBannerContentInfo
            {
                ContainerId = "BannerContainer",
                Key = Random.Range(1, 11)
            };
        }

        private static UIInfoContentInfo GetInfo()
        {
            return new UIInfoContentInfo
            {
                ContainerId = "InfoContainer",
                MainText = " Main Text",
                Tooltip = " ToolTip Text",
                SubText = " Click to Dismiss",
                Title = " Title text",
            };
        }

        private UIMiniatureContentInfo GetRandomMiniatureInfo()
        {
            var miniatureInfo = new UIMiniatureContentInfo();
            int rndInt = Random.Range(0, miniatures.Length);

            miniatureInfo.ContainerId = "MiniatureContainer";
            miniatureInfo.MaxHealth = 100;
            miniatureInfo.Health = Random.Range(1, 100);
            if (rndInt > 0)
            {
                miniatureInfo.MaxMana = 50;
                miniatureInfo.Mana = Random.Range(1, 50);
            }
            miniatureInfo.Picture = miniatures[rndInt];
            miniatureInfo.Selectable = new UIMiniatureSelectable();
            (miniatureInfo.Selectable as UIMiniatureSelectable).Type = rndInt;

            return miniatureInfo;
        }
    }
}

