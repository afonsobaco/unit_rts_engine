using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UIMiniatureSelectionManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] _miniatures;
        private List<UIMiniatureSelectable> _mainList;
     
        private void Start()
        {
            _mainList = new List<UIMiniatureSelectable>();
            for (var i = 0; i < 20; i++)
            {
                for (var j = 0; j < _miniatures.Length; j++)
                {
                    CreateSelectable(j);
                }
            }
        }

        internal List<UIMiniatureContentInfo> GetRandomSelection()
        {
            List<UIMiniatureContentInfo> result = new List<UIMiniatureContentInfo>();
            var size = Random.Range(1, 20);
            for (var i = 0; i < size; i++)
            {
                UIMiniatureContentInfo miniatureInfo = GetRandomMiniatureInfo();
                result.Add(miniatureInfo);
            }
            return result;
        }

        private void CreateSelectable(int type)
        {
            var selectable = new UIMiniatureSelectable();
            selectable.Type = type;
            _mainList.Add(selectable);
        }

        public UIMiniatureContentInfo GetRandomMiniatureInfo()
        {
            var miniatureInfo = new UIMiniatureContentInfo();
            int rndInt = Random.Range(0, _mainList.Count);
            var selectable = _mainList[rndInt];

            miniatureInfo.MaxHealth = 100;
            miniatureInfo.Health = Random.Range(1, 100);
            if (selectable.Type > 0)
            {
                miniatureInfo.MaxMana = 50;
                miniatureInfo.Mana = Random.Range(1, 50);
            }
            miniatureInfo.Picture = _miniatures[selectable.Type];
            miniatureInfo.Selectable = selectable;
            return miniatureInfo;
        }
    }
}
