using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Manager.Abstracts;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Enums;
using RTSEngine.Core.Impls;

namespace RTSEngine.Manager.Impls
{
    [CreateAssetMenu(fileName = "SelectionSettings", menuName = "ScriptableObjects/Selection Settings", order = 1)]
    public class SelectionSettingsSO : AbstractSelectionSettingsSO<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>
    {

    }

}