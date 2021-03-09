using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RTSEngine.RTSUserInterface;
using RTSEngine.Utils;
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
            yield return new WaitForEndOfFrame();
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
            string[] info = GetNextInfo();
            if (info != null)
            {
                button = CreateInfoButton(info);
            }
            return button;
        }

        private DefaultInfoButton CreateInfoButton(string[] info)
        {
            DefaultInfoButton button = _userInterfaceBase.InfoFactory.Create();
            GameUtils.FindInComponent<Text>(button.Title.gameObject).text = info[0];
            GameUtils.FindInComponent<Text>(button.Text.gameObject).text = info[1];
            GameUtils.FindInComponent<Text>(button.SubText.gameObject).text = info[2];
            GameUtils.FindInComponent<Text>(button.ToolTip.gameObject).text = info[3];
            return button;
        }

        private string[] GetNextInfo()
        {
            foreach (var item in _sceneInfoList)
            {
                if (!_infoManager.PanelContainsInfo(item[1]))
                {
                    return item;
                }
            }
            return null;
        }

        private List<string[]> _sceneInfoList = new List<string[]>{
            new string[]{"F1","Show next hint", "click to dismiss", "F1 \n Show next hint"},
            new string[]{"Q/E","Select next/previous model", "click to dismiss", "Q/E \n Select next/previous model"},
            new string[]{"RightClick","Add selected model to scene at mouse positiion", "click to dismiss", "RightClick \n Add selected model to scene at mouse positiion"},
            new string[]{"Shift + RightClick","Remove model on scene at mouse positiion", "click to dismiss", "Shift + RightClick \n Remove model on scene at mouse positiion"},
            new string[]{"Click","Selection", "click to dismiss", "Click \n Selection"},
            new string[]{"Ctrl+Click/DoubleClick","Select all of same model on screen", "click to dismiss", "Ctrl+Click/DoubleClick \n Select all of same model on screen"},
            new string[]{"Shift+Click","Add/Remove from selection", "click to dismiss", "Shift+Click \n Add/Remove from selection"},
            new string[]{"Z + [number]","Create/Remove party at [number] with selection", "click to dismiss", "Z + [number] \n Create/Remove party at [number] with selection"},
        };

    }
}
