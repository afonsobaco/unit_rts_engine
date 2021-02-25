using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;

public class SceneHelperInstaller : MonoInstaller
{
    [Inject]
    private SignalBus _signalBus;

    public override void InstallBindings()
    {
        Container.BindSignal<ChangeSelectionSignal>().ToMethod(UpdateAll);
    }

    private void UpdateAll(ChangeSelectionSignal obj)
    {
        ISelectable[] selection = GameUtils.GetOrderedSelection(obj.Selection, new EqualityComparer(), new ObjectComparer());
        _signalBus.Fire(new SelectionUpdateSignal() { Selection = selection });
        var _helper = GetComponent<SceneHelper>();
        _helper.UpdateAll();
    }

    private class EqualityComparer : IEqualityComparer<ISelectable>
    {
        public bool Equals(ISelectable x, ISelectable y)
        {
            var first = x as MiniatureClass;
            var second = y as MiniatureClass;
            return first.Type == second.Type;
        }

        public int GetHashCode(ISelectable obj)
        {
            var first = obj as MiniatureClass;
            int hCode = first.Type.GetHashCode();
            return hCode;
        }
    }

    private class ObjectComparer : IComparer<IGrouping<ISelectable, ISelectable>>
    {
        public int Compare(IGrouping<ISelectable, ISelectable> x, IGrouping<ISelectable, ISelectable> y)
        {
            return 0;
        }
    }
}
