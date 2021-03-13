using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContent : MonoBehaviour, IInitializable
    {

        [Inject] private UIContainer container;
        private UIContentInfo info;
        public UIContentInfo Info { get => info; set => info = value; }

        public void Initialize()
        {
            Debug.Log("Initialized Content for " + container.ContainerId);
        }

        public virtual void UpdateAppearance()
        {
            Debug.Log("Update appearance ");
        }
    }
}