using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Mod;
using UnityEngine;

namespace RTSEngine.Selection
{
    [CreateAssetMenu(fileName = "SelectionSettings", menuName = "ScriptableObjects/Selection Settings", order = 1)]
    public class SelectionSettingsSO : SelectionSettings<SelectableObject, SelectableTypeEnum>
    {

    }

}