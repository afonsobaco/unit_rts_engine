using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class SceneIntegrationGameType : MonoBehaviour
    {
        [SerializeField] private SceneIntegrationObjectTypeEnum type;

        public SceneIntegrationObjectTypeEnum Type { get => type; set => type = value; }
    }

}