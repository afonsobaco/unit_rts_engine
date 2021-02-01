using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    [CreateAssetMenu(fileName = "SelectableObjectRuntimeSetSO", menuName = "ScriptableObjects/Selectable Runtime Set", order = 1)]
    public class SelectableObjectRuntimeSetSO : AbstractRuntimeSetSO<ISelectableObjectBehaviour>
    {

    }
}
