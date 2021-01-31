
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


        public static ISelectableObjectBehaviour CreateATestableObject(int i)
        {
            var obj = Substitute.For<ISelectableObjectBehaviour>();
            obj.Position.Returns(GetDefaultTestListOfObjects().ElementAt(i).Pos);
            return obj;
        }

        public static SelectionArgsXP GetDefaultArgs()
        {
            return GetDefaultArgs(new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>(), new HashSet<ISelectableObjectBehaviour>());
        }

        public static SelectionArgsXP GetDefaultArgs(HashSet<ISelectableObjectBehaviour> oldSelection, HashSet<ISelectableObjectBehaviour> newSelection, HashSet<ISelectableObjectBehaviour> mainList)
        {
            SelectionArgsXP args = new SelectionArgsXP(oldSelection, newSelection, mainList);
            return args;
        }

        public static HashSet<IBaseSelectionMod> GetSomeModsFromType(int amount, SelectionTypeEnum type)
        {
            HashSet<IBaseSelectionMod> list = new HashSet<IBaseSelectionMod>();
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

