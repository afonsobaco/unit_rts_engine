using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RTSEngine.RTSUserInterface;
using Zenject;
using System;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneInfoInput : MonoBehaviour
    {
        [SerializeField] private KeyCode helpKeyCode = KeyCode.F1;

        [Inject] private UserInterfaceBase _userInterfaceBase;

        private DefaultUserInterfaceInfoManager _infoManager;

        private void Start()
        {
            var infoPanel = _userInterfaceBase.UserInterfaceBaseComponent.InfoPanel;
            if (infoPanel)
            {
                _infoManager = infoPanel.GetComponent<DefaultUserInterfaceInfoManager>();
            }

            StartCoroutine(StartInfo());
        }

        private IEnumerator StartInfo()
        {
            yield return new WaitForSeconds(0);
            foreach (var info in _sceneInfoList)
            {
                ExecuteEvents.Execute<IInfoMessageTarget>(_infoManager.gameObject, null, (x, y) => x.AddInfo(CreateInfoButton(info)));
            }
        }

        private void Update()
        {
            if (_infoManager && Input.GetKeyDown(helpKeyCode))
            {
                AddInfo();
            }
        }

        private void AddInfo()
        {
            DefaultInfoButton button = GetInfoButton();
            if (button)
            {
                ExecuteEvents.Execute<IInfoMessageTarget>(_infoManager.gameObject, null, (x, y) => x.AddInfo(button));
            }
        }

        private DefaultInfoButton GetInfoButton()
        {
            DefaultInfoButton button = null;
            object[] info = GetNextInfo();
            if (info != null)
            {
                button = CreateInfoButton(info);
            }
            return button;
        }

        private DefaultInfoButton CreateInfoButton(object[] info)
        {
            DefaultInfoButton button = _userInterfaceBase.InfoFactory.Create();
            button.Title.text = info[0] as string;
            button.Text.text = info[1] as string;
            button.SubText.text = info[2] as string;
            button.ToolTip.text = info[3] as string;
            return button;
        }

        private object[] GetNextInfo()
        {
            foreach (var item in _sceneInfoList)
            {
                if (!_infoManager.PanelContainsInfo(item[1] as string))
                {
                    return item;
                }
            }
            return null;
        }

        private List<object[]> _sceneInfoList = new List<object[]>{
            new object[]{"F1","Show next hint", "click to dismiss", "tooltip"},
            new object[]{"Q/E","Select next/previous model", "click to dismiss", "tooltip"},
            new object[]{"RightClick","Add selected model to scene at mouse positiion", "click to dismiss", "tooltip"},
            new object[]{"Shift + RightClick","Remove model on scene at mouse positiion", "click to dismiss", "tooltip"},
            new object[]{"Click","Selection", "click to dismiss", "tooltip"},
            new object[]{"Ctrl+Click/DoubleClick","Select all of same model on screen", "click to dismiss", "tooltip"},
            new object[]{"Shift+Click","Add/Remove from selection", "click to dismiss", "tooltip"},
            new object[]{"Z + [number]","Create/Remove party at [number] with selection", "click to dismiss", "tooltip"},
        };

    }
}
