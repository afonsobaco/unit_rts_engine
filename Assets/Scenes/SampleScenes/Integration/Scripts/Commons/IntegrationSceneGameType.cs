using UnityEngine;

namespace RTSEngine.Integration.Scene
{

    public class IntegrationSceneGameType : MonoBehaviour
    {
        [SerializeField] private IntegrationSceneObjectTypeEnum type;

        public IntegrationSceneObjectTypeEnum Type { get => type; set => type = value; }
    }

}