using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;


namespace RTSEngine.Refactoring
{

    [CreateAssetMenu(fileName = "ModifiersSO", menuName = "Installers/ModifiersSO")]
    public class ModifiersSO : ScriptableObject
    {
        [SerializeField] private BaseSelectionModifier[] modifiers;

    }
}