using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    GameObject ShipPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameRunner>().AsSingle();

        Container.BindFactory<float, ShipFacade, ShipFacade.Factory>().FromSubContainerResolve().ByNewPrefabInstaller<ShipInstaller>(ShipPrefab);
    }
}