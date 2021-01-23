
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager;

using NSubstitute;

namespace Tests.Manager
{
    public class SelectionManagerTestUtils
    {
        public static List<SelectableObjectTestStruct> GetDefaultTestListOfObjects()
        {
            return new List<SelectableObjectTestStruct>(){
                new SelectableObjectTestStruct(new Vector3(0,0,0), ObjectTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(1,0,1), ObjectTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(-1,0,1), ObjectTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(-1,0,-1), ObjectTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(2,0,3), ObjectTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(4,0,3), ObjectTypeEnum.BUILDING),
                new SelectableObjectTestStruct(new Vector3(2,0,-3), ObjectTypeEnum.BUILDING),
                new SelectableObjectTestStruct(new Vector3(1,0,4), ObjectTypeEnum.ENVIRONMENT),
                new SelectableObjectTestStruct(new Vector3(4,0,-2), ObjectTypeEnum.CONSUMABLE)
            };
        }


        public static T CreateATestableObject<T>(int i) where T : SelectableObject
        {
            GameObject go = new GameObject();
            var t = go.AddComponent<T>();
            var bc = go.AddComponent<BoxCollider>();
            t.transform.position = GetDefaultTestListOfObjects()[i].Pos;
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

        public static ISelectionArgsXP<T, ST> GetDefaultArgs<T, ST>()
        {
            ISelectionArgsXP<T, ST> args = new SelectionArgsXP<T, ST>();
            args.NewSelection = new List<T>();
            args.OldSelection = new List<T>();
            args.ToBeAdded = new List<T>();
            args.ToBeRemoved = new List<T>();
            return args;
        }

        public static T CreateGameObject<T>() where T : MonoBehaviour
        {
            var go = new GameObject();
            var so = go.AddComponent<T>();
            return so;
        }

        public static List<ISelectionMod<O, ST>> GetSomeMods<O, ST>(int amount)
        {
            List<ISelectionMod<O, ST>> mods = new List<ISelectionMod<O, ST>>();
            for (var i = 0; i < amount; i++)
            {
                ISelectionMod<O, ST> mod = Substitute.For<ISelectionMod<O, ST>>();
                mod.Apply(Arg.Any<SelectionArgsXP<O, ST>>()).Returns(x => x[0]);
                mods.Add(mod);
            }
            return mods;
        }

    }
}

public struct SelectableObjectTestStruct
{
    private Vector3 pos;
    private ObjectTypeEnum typeEnum;

    internal Vector3 Pos { get => pos; }
    internal ObjectTypeEnum TypeEnum { get => typeEnum; }

    internal SelectableObjectTestStruct(Vector3 pos, ObjectTypeEnum typeEnum)
    {
        this.pos = pos;
        this.typeEnum = typeEnum;
    }
}

