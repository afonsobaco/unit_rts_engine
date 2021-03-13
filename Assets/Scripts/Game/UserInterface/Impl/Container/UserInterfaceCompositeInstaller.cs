using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.RTSUserInterface
{

    [CreateAssetMenu(fileName = "UserInterfaceCompositeInstaller", menuName = "RTS Engine/UserInterfaceCompositeInstaller", order = 0)]
    public class UserInterfaceCompositeInstaller : ScriptableObjectInstaller<UserInterfaceCompositeInstaller>
    {
        [SerializeField] public List<UserInterfaceContainerInstaller> _containers;

        public override void InstallBindings()
        {
            foreach (var item in _containers)
            {
                Container.Inject(item);
                item.InstallBindings();
            }
        }
    }
}