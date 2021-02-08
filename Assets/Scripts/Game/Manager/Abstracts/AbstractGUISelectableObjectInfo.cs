using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Manager
{

    public class AbstractGUISelectableObjectInfo : ScriptableObject, IGUISelectableObjectInfo
    {

        [Space]
        [Header("Selectable Info")]
        [SerializeField] private ObjectTypeEnum type;
        [SerializeField] private Sprite picture;
        [Space]
        [SerializeField] int life;
        [SerializeField] int mana;

        [Space]
        [SerializeField] private string objectName;  //TODO should be an array of types eg.: [Human, Wizzard, Level2, etc...]
        [SerializeField] private int selectionOrder;

        public ObjectTypeEnum Type { get => type; set => type = value; }
        public Sprite Picture { get => picture; set => picture = value; }
        public string ObjectName { get => objectName; set => objectName = value; }
        public int SelectionOrder { get => selectionOrder; set => selectionOrder = value; }

        public int Life { get => life; set => life = value; }
        public int Mana { get => mana; set => mana = value; }
    }
}
