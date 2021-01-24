
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


        public static SelectableObject CreateATestableObject(int i)
        {
            GameObject go = new GameObject();
            var t = go.AddComponent<SelectableObject>();
            var bc = go.AddComponent<BoxCollider>();
            t.transform.position = GetDefaultTestListOfObjects()[i].Pos;
            return t;
        }

        public static List<SelectableObject> GetDefaultTestMainList()
        {
            List<SelectableObject> mainList = new List<SelectableObject>();

            for (var i = 0; i < GetDefaultTestListOfObjects().Count; i++)
            {
                var t = CreateATestableObject(i);
                mainList.Add(t);
            }
            return mainList;
        }

        public static SelectionArgsXP GetDefaultArgs()
        {
            SelectionArgsXP args = new SelectionArgsXP();
            args.NewSelection = new List<SelectableObject>();
            args.OldSelection = new List<SelectableObject>();
            args.ToBeAdded = new List<SelectableObject>();
            args.ToBeRemoved = new List<SelectableObject>();
            return args;
        }

        public static SelectableObject CreateGameObject()
        {
            var go = new GameObject();
            var so = go.AddComponent<SelectableObject>();
            return so;
        }

        public static List<IBaseSelectionMod> GetSomeModsFromType(int amount, SelectionTypeEnum type)
        {
            List<IBaseSelectionMod> list = new List<IBaseSelectionMod>();
            for (var i = 0; i < amount; i++)
            {
                IBaseSelectionMod mod = Substitute.For<IBaseSelectionMod>();
                mod.Type.Returns(type);
                mod.Active.Returns(true);
                mod.Apply(Arg.Any<SelectionArgsXP>()).Returns(x => x[0]);
                list.Add(mod);
            }
            return list;
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

