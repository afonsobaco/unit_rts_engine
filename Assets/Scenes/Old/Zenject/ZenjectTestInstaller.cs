using System;
using UnityEngine;
using Zenject;

public class ZenjectTestInstaller : MonoInstaller
{

    [SerializeField] private GameObject prefab;

    public override void InstallBindings()
    {
        Container.BindFactory<Foo, Foo.Factory>().FromComponentInNewPrefab(prefab).OnInstantiated(Created);
    }

    private void Created(InjectContext arg1, object arg2)
    {
        Debug.Log("created");
    }
}
