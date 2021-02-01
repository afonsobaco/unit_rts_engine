using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    [CreateAssetMenu(fileName = "SelectableObjectRuntimeSetSO", menuName = "ScriptableObjects/ISelectable Runtime Set", order = 1)]
    public class SelectableObjectRuntimeSetSO : AbstractRuntimeSetSO<ISelectableObjectBehaviour>
    {

    }
}
