using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;
using RTSEngine.Commons;

public class SceneHelperInstaller : MonoInstaller
{
    [Inject] private SignalBus _signalBus;

    public override void InstallBindings()
    {
        Container.DeclareSignal<ChangeSelectionSignal>();
        Container.BindSignal<ChangeSelectionSignal>().ToMethod<SceneHelper>(x => x.UpdateAll).FromResolve();
    }


}
