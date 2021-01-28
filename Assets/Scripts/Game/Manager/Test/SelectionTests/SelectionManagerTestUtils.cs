
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager;
using RTSEngine.Core;

using NSubstitute;

namespace Tests.Manager
{
    public class SelectionManagerTestUtils
    {
        private static List<SelectableObjectTestStruct> GetDefaultTestListOfObjects()
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


        public static ISelectable CreateATestableObject(int i)
        {
            var obj = Substitute.For<ISelectable>();
            obj.Position.Returns(GetDefaultTestListOfObjects()[i].Pos);
            return obj;
        }

        public static SelectionArgsXP GetDefaultArgs()
        {
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments();
            return GetDefaultArgs(arguments, modifierArguments);
        }

        public static SelectionArgsXP GetDefaultArgs(SelectionArguments arguments)
        {
            SelectionModifierArguments modifierArguments = new SelectionModifierArguments();
            return GetDefaultArgs(arguments, modifierArguments);
        }

        public static SelectionArgsXP GetDefaultArgs(SelectionModifierArguments modifierArguments)
        {
            SelectionArguments arguments = new SelectionArguments(SelectionTypeEnum.ANY, false, new List<ISelectable>(), new List<ISelectable>(), new List<ISelectable>());
            return GetDefaultArgs(arguments, modifierArguments);
        }

        public static SelectionArgsXP GetDefaultArgs(SelectionArguments arguments, SelectionModifierArguments modifierArguments)
        {
            SelectionArgsXP args = new SelectionArgsXP(arguments, modifierArguments);
            return args;
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

