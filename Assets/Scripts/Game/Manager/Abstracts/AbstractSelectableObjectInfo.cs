using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Manager
{

    public class AbstractSelectableObjectInfo : ScriptableObject, ISelectableObjectInfo
    {

        [Space]
        [Header("Selectable Info")]
        [SerializeField] private ObjectTypeEnum type;
        [SerializeField] private Sprite picture;
        [Space]
        [SerializeField] ObjectStatus life;
        [SerializeField] ObjectStatus mana;

        [Space]
        [SerializeField] private string typeStr;  //TODO should be an array of types eg.: [Human, Wizzard, Level2, etc...]
        [SerializeField] private int selectionOrder;

        public ObjectTypeEnum Type { get => type; set => type = value; }
        public Sprite Picture { get => picture; set => picture = value; }
        public string TypeStr { get => typeStr; set => typeStr = value; }
        public int SelectionOrder { get => selectionOrder; set => selectionOrder = value; }


        public ObjectStatus Life { get => life; set => life = value; }
        public ObjectStatus Mana { get => mana; set => mana = value; }
    }
}
