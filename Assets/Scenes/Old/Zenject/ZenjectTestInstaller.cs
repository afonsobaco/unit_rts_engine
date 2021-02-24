using System;
using UnityEngine;
using Zenject;

public class ZenjectTestInstaller : MonoInstaller
{

    [SerializeField] private Foo foo;

    public override void InstallBindings()
    {
        // Container.BindFactory<Foo, Foo.Factory>().FromComponentInNewPrefab(prefab).OnInstantiated(Created);
        Container.Bind<Foo>().FromComponentInNewPrefab(foo).AsSingle().NonLazy();
    }

}
