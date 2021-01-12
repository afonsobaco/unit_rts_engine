using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RTSEngine.Manager;
using RTSEngine.Selection.Mod;
using RTSEngine.Core;
using RTSEngine.Selection;
using NSubstitute;
using System;

namespace Tests
{
    public class SelectionManagerTest
    {

        private SelectionManagerXP<SelectableObject, SelectionTypeEnum> manager;

        private List<SelectableObjectTestStruct> defaultMainListStruct = new List<SelectableObjectTestStruct>(){
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

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithNull()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            List<SelectableObject> oldSelection = null;
            List<SelectableObject> newSelection = null;
            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            AssertArgs(GetDefaultArgs<SelectableObject, SelectionTypeEnum>(), args);
        }

        [Test]
        public void ShouldReturnDefaultArgsWhenGetSelectionArgsWithEmpty()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            List<SelectableObject> oldSelection = new List<SelectableObject>();
            List<SelectableObject> newSelection = new List<SelectableObject>();

            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.DRAG);

            AssertArgs(GetDefaultArgs<SelectableObject, SelectionTypeEnum>(), args);
        }

        [Test]
        public void ShouldReturnCustomArgsWhenGetSelectionArgsWithCustom()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            List<SelectableObject> oldSelection = new List<SelectableObject>() { CreateATestableObject<SelectableObject>(0) };
            List<SelectableObject> newSelection = new List<SelectableObject>() { CreateATestableObject<SelectableObject>(1) };
            SelectionArgsXP<SelectableObject, SelectionTypeEnum> expected = GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            expected.SelectionType = SelectionTypeEnum.CLICK;
            expected.OldSelection = oldSelection;
            expected.NewSelection = newSelection;

            var args = manager.GetSelectionArgs(oldSelection, newSelection, SelectionTypeEnum.CLICK);

            AssertArgs(expected, args);
        }

        [Test]
        public void ShouldGetModsBySelectionType()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>> mods = GetModsToTest();
            SelectionTypeEnum type = SelectionTypeEnum.CLICK;
            mods[0].Type = type;
            mods[2].Type = type;

            var result = manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains(mods[0]));
            Assert.True(result.Contains(mods[2]));
        }


        [Test]
        public void ShouldGetEmptyWhenModsDoesNotContainsSelectionType()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>> mods = GetModsToTest();
            SelectionTypeEnum type = SelectionTypeEnum.CLICK;

            var result = manager.GetModsBySelectionType(mods, type);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void ShouldApplyModsToArgs()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>> mods = GetModsToTest();
            mods[0].Type = SelectionTypeEnum.CLICK;

            var args = GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            args.Settings = Substitute.For<ISelectionSettingsSO<SelectableObject, SelectionTypeEnum>>();
            args.Settings.Mods = mods;
            args.SelectionType = SelectionTypeEnum.DRAG;

            var result = manager.ApplyModifiers(args);

            foreach (var mod in args.Settings.Mods)
            {
                if (mod.Type.Equals(args.SelectionType))
                {
                    mod.Received().Apply(Arg.Any<SelectionArgsXP<SelectableObject, SelectionTypeEnum>>());
                }
                else
                {
                    mod.DidNotReceive().Apply(Arg.Any<SelectionArgsXP<SelectableObject, SelectionTypeEnum>>());
                }
            }

        }


        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeAdded()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            var args = GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            var amount = 4;
            List<SelectableObject> selection = new List<SelectableObject>();
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < amount; i++)
            {
                var obj = CreateATestableObject<SelectableObject>(i);
                selection.Add(obj);
                expected.Add(obj);
            }
            args.NewSelection = selection;
            args.ToBeAdded = selection;

            var result = manager.FinalizeSelection(args);

            Assert.AreEqual(expected, result);
            foreach (var item in result)
            {
                Assert.True(item.IsSelected);
            }
        }

        [Test]
        public void ShouldFinalizeSelectionCorrectlyWhenHasSomethingToBeRemoved()
        {
            manager = new SelectionManagerXP<SelectableObject, SelectionTypeEnum>();
            var args = GetDefaultArgs<SelectableObject, SelectionTypeEnum>();
            var amount = 4;
            List<SelectableObject> expected = new List<SelectableObject>();
            for (var i = 0; i < amount; i++)
            {
                var obj = CreateATestableObject<SelectableObject>(i);
                args.NewSelection.Add(obj);
                if (i > 1){

                    args.ToBeAdded.Add(obj);
                    expected.Add(obj);
                }else{
                    args.ToBeRemoved.Add(obj);
                }
            }                      

            var result = manager.FinalizeSelection(args);

            CollectionAssert.AreEquivalent(expected, result);
            foreach (var item in args.ToBeAdded)
            {
                Assert.True(item.IsSelected);
            }            
            foreach (var item in args.NewSelection.FindAll(a => !result.Contains(a)))
            {
                Assert.False(item.IsSelected);
            }
        }

        #region methods

        private static List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>> GetModsToTest()
        {
            List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>> mods = new List<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>>();
            mods.Add(Substitute.For<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>>());
            mods.Add(Substitute.For<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>>());
            mods.Add(Substitute.For<IAbstractSelectionMod<SelectableObject, SelectionTypeEnum>>());

            foreach (var mod in mods)
            {
                mod.Apply(Arg.Any<SelectionArgsXP<SelectableObject, SelectionTypeEnum>>()).Returns(x => x[0]);
            }

            return mods;
        }

        private void AssertArgs<T, E>(SelectionArgsXP<T, E> expected, SelectionArgsXP<T, E> actual)
        {
            CollectionAssert.AreEquivalent(expected.OldSelection, actual.OldSelection);
            CollectionAssert.AreEquivalent(expected.NewSelection, actual.NewSelection);
            CollectionAssert.AreEquivalent(expected.ToBeAdded, actual.ToBeAdded);
            CollectionAssert.AreEquivalent(expected.ToBeRemoved, actual.ToBeRemoved);
            Assert.AreEqual(expected.SelectionType, actual.SelectionType);
            // Assert.AreEqual(expected.Settings, actual.Settings);
        }


        private IAbstractSelectionMod<T, E> AddNewMod<T, E>(SelectionArgsXP<T, E> args)
        {
            var mod = Substitute.For<IAbstractSelectionMod<T, E>>();
            if (args.Settings == null)
            {
                args.Settings = Substitute.For<ISelectionSettingsSO<T, E>>();
            }
            if (args.Settings.Mods == null)
            {
                args.Settings.Mods = new List<IAbstractSelectionMod<T, E>>();
            }
            args.Settings.Mods.Add(mod);
            return mod;
        }
        private SelectionArgsXP<T, E> GetDefaultArgs<T, E>()
        {
            SelectionArgsXP<T, E> args = new SelectionArgsXP<T, E>();
            args.Settings = Substitute.For<ISelectionSettingsSO<T, E>>();
            return args;
        }

        private List<T> GetDefaultTestMainList<T>() where T : MonoBehaviour
        {
            List<T> mainList = new List<T>();

            for (var i = 0; i < defaultMainListStruct.Count; i++)
            {
                var t = CreateATestableObject<T>(i);
                mainList.Add(t);
            }
            return mainList;
        }

        private T CreateATestableObject<T>(int i) where T : MonoBehaviour
        {
            GameObject go = new GameObject();
            var t = go.AddComponent<T>();
            t.transform.position = defaultMainListStruct[i].pos;
            return t;
        }
        #endregion
    }

    internal struct SelectableObjectTestStruct
    {
        internal Vector3 pos;
        internal SelectableTypeEnum typeEnum;

        internal SelectableObjectTestStruct(Vector3 pos, SelectableTypeEnum typeEnum)
        {
            this.pos = pos;
            this.typeEnum = typeEnum;
        }
    }
}
