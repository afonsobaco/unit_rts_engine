using RTSEngine.Core.Enums;
using RTSEngine.Core.Interfaces;
using RTSEngine.Utils;
using UnityEngine;

namespace RTSEngine.Core.Impls
{
    public class SelectableObject : MonoBehaviour, ISelectableObject
    {
        private bool _selected = false;
        public ObjectTypeEnum type;
        public SelectionMark selectionMark;
        public SelectionMark preSelectionMark;

        //TODO should be an Enum?
        public string typeStr;

        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                if (selectionMark)
                    selectionMark.transform.gameObject.SetActive(value);
                _selected = value;
            }
        }
        private bool _preSelected = false;

        public bool IsPreSelected
        {
            get { return _preSelected; }
            set
            {
                if (preSelectionMark)
                    preSelectionMark.transform.gameObject.SetActive(value);
                _preSelected = value;
            }
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

    }

}
