using UnityEngine;

namespace RTSEngine.Manager
{

    public abstract class AbstractSelectionSettings : ScriptableObject, ISelectionSettings
    {

        [Space]
        [Header("Order of selection")]
        [SerializeField] private ObjectTypeEnum[] primary;
        [SerializeField] private ObjectTypeEnum[] secondary;

        [Space]
        [Header("Same type selected")]
        [SerializeField] private ObjectTypeEnum[] canGroup;
        [SerializeField] private SameTypeSelectionModeEnum mode;

        [Space]
        [Header("Limit of selection")]
        [SerializeField] private int limit;

        public int Limit => limit;
        public ObjectTypeEnum[] CanGroup => canGroup;
        public ObjectTypeEnum[] Primary => primary;
        public ObjectTypeEnum[] Secondary => secondary;
        public SameTypeSelectionModeEnum Mode => mode;
    }

}