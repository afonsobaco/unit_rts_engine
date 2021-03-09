using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultInfoButton : DefaultClickable
    {
        [SerializeField] private GameObject title;
        [SerializeField] private GameObject text;
        [SerializeField] private GameObject subText;
        [SerializeField] private GameObject toolTip;
        [SerializeField] private GameObject picture;
        [SerializeField] private float _creationAnimTime = .5f;
        [SerializeField] private float _destroyAnimTime = .5f;

        public GameObject Title { get => title; set => title = value; }
        public GameObject Text { get => text; set => text = value; }
        public GameObject SubText { get => subText; set => subText = value; }
        public GameObject ToolTip { get => toolTip; set => toolTip = value; }
        public GameObject Picture { get => picture; set => picture = value; }
        public float CreationAnimTime { get => _creationAnimTime; set => _creationAnimTime = value; }
        public float DestroyAnimTime { get => _destroyAnimTime; set => _destroyAnimTime = value; }

        public override void DoClick()
        {
            ExecuteEvents.Execute<IInfoMessageTarget>(this.transform.parent.gameObject, null, (x, y) => x.RemoveInfo(this));
        }

        public virtual IEnumerator DoDestroyAnim()
        {
            yield return null;
        }

        public virtual IEnumerator DoCreateAnim()
        {
            yield return null;
        }

        public class Factory : PlaceholderFactory<DefaultInfoButton>
        {
        }


    }
}
