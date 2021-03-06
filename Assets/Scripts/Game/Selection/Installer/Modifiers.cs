using UnityEngine;
using System.Linq;

/*
Sample order reminder:
    SubGroup
    TypePriority
    Additive
    DoubleClick
    CanBeGrouped
    Limit
    Sort
*/

namespace RTSEngine.RTSSelection
{

    [CreateAssetMenu(fileName = "Modifiers", menuName = "Installers/Modifiers")]
    public class Modifiers : ScriptableObject, IModifiersComponent
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