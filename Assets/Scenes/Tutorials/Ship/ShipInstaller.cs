using UnityEngine;
using Zenject;

public class ShipInstaller : Installer<ShipInstaller>
{
    readonly float _speed;

    public ShipInstaller(
        [InjectOptional]
        float speed)
    {
        _speed = speed;
    }

    public override void InstallBindings()
    {
        Container.Bind<ShipFacade>().AsSingle();
        Container.Bind<Transform>().FromComponentOnRoot();
        Container.BindInterfacesTo<ShipInputHandler>().AsSingle();
        Container.BindInstance(_speed).WhenInjectedInto<ShipInputHandler>();
        Container.Bind<ShipHealthHandler>().FromNewComponentOnRoot().AsSingle();
    }
}