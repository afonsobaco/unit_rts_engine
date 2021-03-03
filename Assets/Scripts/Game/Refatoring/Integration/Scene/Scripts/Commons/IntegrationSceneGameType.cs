using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class IntegrationSceneGameType : MonoBehaviour
    {
        [SerializeField] private IntegrationSceneObjectTypeEnum type;

        public IntegrationSceneObjectTypeEnum Type { get => type; set => type = value; }
    }

}