using UnityEngine;
using System.Linq;


namespace RTSEngine.Refactoring
{

    [CreateAssetMenu(fileName = "ModifiersSO", menuName = "Installers/ModifiersSO")]
    public class ModifiersSO : ScriptableObject, IModifiersComponent
    {
        [SerializeField] private BaseSelectionModifier[] modifiers;

        public ISelectionModifier[] GetModifiers()
        {
            var selectionModifiers = modifiers.ToList();
            selectionModifiers.RemoveAll(x => !x.Active);
            return selectionModifiers.ToArray();
        }
    }
}