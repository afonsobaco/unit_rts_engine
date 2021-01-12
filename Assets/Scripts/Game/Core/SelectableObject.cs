using UnityEngine;

namespace RTSEngine.Core
{
    public class SelectableObject : MonoBehaviour
    {
        private bool _selected = false;
        public SelectableTypeEnum type;
        public SelectionMark selectionMark;
        public SelectionMark preSelectionMark;

        //TODO should be an Enum?
        public string typeStr;

        public bool IsSelected
        {
            get { return _selected; }
            set
            {
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
                preSelectionMark.transform.gameObject.SetActive(value);
                _preSelected = value;
            }
        }

        void OnEnable()
        {
            SelectableObjectMainList.Instance.AddToMainList(this);
        }

        void OnDisable()
        {
            SelectableObjectMainList.Instance.RemoveFromMainList(this);
        }

    }
}
