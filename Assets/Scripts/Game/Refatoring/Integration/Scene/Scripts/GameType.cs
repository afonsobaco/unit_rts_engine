using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class GameType : MonoBehaviour
    {
        [SerializeField] private ObjectTypeEnum type;

        public ObjectTypeEnum Type { get => type; set => type = value; }
    }

}