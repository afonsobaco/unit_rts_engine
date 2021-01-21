
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Manager.Impls;

using NSubstitute;

namespace Tests.Manager
{
    public class SelectionManagerTestUtils
    {
        public static List<SelectableObjectTestStruct> GetDefaultTestListOfObjects()
        {
            return new List<SelectableObjectTestStruct>(){
                new SelectableObjectTestStruct(new Vector3(0,0,0), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(1,0,1), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(-1,0,1), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(-1,0,-1), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(2,0,3), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(4,0,3), SelectableTypeEnum.BUILDING),
                new SelectableObjectTestStruct(new Vector3(2,0,-3), SelectableTypeEnum.BUILDING),
                new SelectableObjectTestStruct(new Vector3(1,0,4), SelectableTypeEnum.ENVIRONMENT),
                new SelectableObjectTestStruct(new Vector3(4,0,-2), SelectableTypeEnum.CONSUMABLE)
            };
        }


        public static T CreateATestableObject<T>(int i) where T : SelectableObject
        {
            GameObject go = new GameObject();
            var t = go.AddComponent<T>();
            var bc = go.AddComponent<BoxCollider>();
            t.transform.position = GetDefaultTestListOfObjects()[i].pos;
            return t;
        }

        public static List<T> GetDefaultTestMainList<T>() where T : SelectableObject
        {
            List<T> mainList = new List<T>();

            for (var i = 0; i < GetDefaultTestListOfObjects().Count; i++)
            {
                var t = CreateATestableObject<T>(i);
                mainList.Add(t);
            }
            return mainList;
        }

        public static IAbstractSelectionMod<T, E> AddNewMod<T, E>(SelectionArgsXP<T, E> args)
        {
            var mod = Substitute.For<IAbstractSelectionMod<T, E>>();
            if (args.Settings == null)
            {
                args.Settings = Substitute.For<ISelectionSettings<T, E>>();
            }
            if (args.Settings.Mods == null)
            {
                args.Settings.Mods = new List<IAbstractSelectionMod<T, E>>();
            }
            args.Settings.Mods.Add(mod);
            return mod;
        }

        public static SelectionArgsXP<T, E> GetDefaultArgs<T, E>()
        {
            SelectionArgsXP<T, E> args = new SelectionArgsXP<T, E>();
            args.Settings = Substitute.For<ISelectionSettings<T, E>>();
            args.NewSelection = new List<T>();
            args.OldSelection = new List<T>();
            args.ToBeAdded = new List<T>();
            args.ToBeRemoved = new List<T>();
            args.Settings = Substitute.For<ISelectionSettings<T, E>>();
            args.Settings.Mods = new List<IAbstractSelectionMod<T, E>>();
            return args;
        }

        public static T CreateGameObject<T>() where T : MonoBehaviour
        {
            var go = new GameObject();
            var so = go.AddComponent<T>();
            return so;
        }
    }
}

public struct SelectableObjectTestStruct
{
    internal Vector3 pos;
    internal SelectableTypeEnum typeEnum;

    internal SelectableObjectTestStruct(Vector3 pos, SelectableTypeEnum typeEnum)
    {
        this.pos = pos;
        this.typeEnum = typeEnum;
    }
}

