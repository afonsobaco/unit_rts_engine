using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.RTSSelection.Scene
{
    public class SelectionSceneGameType : MonoBehaviour
    {
        [SerializeField] private string type;
        public string Type { get => type; set => type = value; }
    }

}