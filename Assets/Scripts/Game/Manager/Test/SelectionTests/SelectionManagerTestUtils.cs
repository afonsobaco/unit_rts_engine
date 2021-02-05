
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager;
using RTSEngine.Core;

using NSubstitute;

namespace Tests.Manager
{
    public class SelectionManagerTestUtils
    {
        private static HashSet<SelectableObjectTestStruct> GetDefaultTestListOfObjects()
        {
            return new HashSet<SelectableObjectTestStruct>(){
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


        public static ISelectableObject CreateATestableObject(int i)
        {
            var obj = Substitute.For<ISelectableObject>();
            obj.Position.Returns(GetDefaultTestListOfObjects().ElementAt(i).Pos);
            return obj;
        }

        public static SelectionArgsXP GetDefaultArgs()
        {
            return GetDefaultArgs(new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>(), new HashSet<ISelectableObject>());
        }

        public static SelectionArgsXP GetDefaultArgs(HashSet<ISelectableObject> oldSelection, HashSet<ISelectableObject> newSelection, HashSet<ISelectableObject> mainList)
        {
            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);
            return args;
        }

        public static List<ISelectionModifier> GetSomeModsFromType(int amount, SelectionTypeEnum type)
        {
            List<ISelectionModifier> list = new List<ISelectionModifier>();
            for (var i = 0; i < amount; i++)
            {
                ISelectionModifier mod = Substitute.For<ISelectionModifier>();
                mod.Type.Returns(type);
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

