using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring.Scene.Selection
{
    public class SelectionSceneGameType : MonoBehaviour
    {
        [SerializeField] private string type;
        public string Type { get => type; set => type = value; }
    }

}