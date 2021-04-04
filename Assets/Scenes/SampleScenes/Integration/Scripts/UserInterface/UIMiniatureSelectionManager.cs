// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using RTSEngine.Core;
// using Zenject;

// namespace RTSEngine.Integration.Scene
// {
//     public class UIMiniatureSelectionManager : MonoBehaviour
//     {
//         [SerializeField] private Sprite[] _miniatures;
//         private List<UISceneIntegratedSelectable> _mainList;

//         private void Start()
//         {
//             _mainList = new List<UISceneIntegratedSelectable>();
//             for (var i = 0; i < 20; i++)
//             {
//                 for (var j = 0; j < _miniatures.Length; j++)
//                 {
//                     CreateSelectable(j);
//                 }
//             }
//         }

//         internal List<UISceneIntegratedContentInfo> GetRandomSelection()
//         {
//             List<UISceneIntegratedContentInfo> result = new List<UISceneIntegratedContentInfo>();
//             var size = Random.Range(1, 20);
//             for (var i = 0; i < size; i++)
//             {
//                 UISceneIntegratedContentInfo miniatureInfo = GetRandomMiniatureInfo();
//                 result.Add(miniatureInfo);
//             }
//             return result;
//         }

//         private void CreateSelectable(int type)
//         {
//             var selectable = new UISceneIntegratedSelectable();
//             selectable.Type = type;
//             _mainList.Add(selectable);
//         }

//         public UISceneIntegratedContentInfo GetRandomMiniatureInfo()
//         {
//             int rndInt = Random.Range(0, _mainList.Count);
//             var selectable = _mainList[rndInt];

//             selectable.MaxHealth = 100;
//             selectable.Health = Random.Range(10, 100);
//             if (selectable.Type > 0)
//             {
//                 selectable.MaxMana = 50;
//                 selectable.Mana = Random.Range(10, 50);
//             }
//             selectable.Picture = _miniatures[selectable.Type];

//             var miniatureInfo = new UISceneIntegratedContentInfo();
//             miniatureInfo.Selectable = selectable;

//             return miniatureInfo;
//         }
//     }
// }
