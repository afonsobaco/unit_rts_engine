using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.Selection.Mod
{
    [CreateAssetMenu(fileName = "SelectionMods", menuName = "ScriptableObjects/Selection Mods", order = 2)]
    public class SelectionModsSO : MonoBehaviour
    {
        [SerializeField] private List<AbstractSelectionMod> mods = new List<AbstractSelectionMod>();
    }
}
