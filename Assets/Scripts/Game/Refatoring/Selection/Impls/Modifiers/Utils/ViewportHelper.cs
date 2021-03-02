using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Commons;
using System;

namespace RTSEngine.Refactoring
{
    [CreateAssetMenu(fileName = "ViewportHelper", menuName = "Installers/ViewportHelper")]

    public class ViewportHelper : ScriptableObject, IViewportHelper
    {
        [SerializeField] private Vector2 _initialViewportPoint = Vector2.zero;
        [SerializeField] private Vector2 _finalViewportPoint = Vector2.one;
        public Vector2 InitialViewportPoint { get => _initialViewportPoint; set => _initialViewportPoint = value; }
        public Vector2 FinalViewportPoint { get => _finalViewportPoint; set => _finalViewportPoint = value; }
    }
}