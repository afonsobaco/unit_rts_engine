using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultInfoButton : DefaultClickable
    {
        [SerializeField] private Text title;
        [SerializeField] private Text text;
        [SerializeField] private Text subText;
        [SerializeField] private Text toolTip;
        [SerializeField] private Image picture;

        public Text Title { get => title; set => title = value; }
        public Text Text { get => text; set => text = value; }
        public Text SubText { get => subText; set => subText = value; }
        public Text ToolTip { get => toolTip; set => toolTip = value; }
        public Image Picture { get => picture; set => picture = value; }

        public override void DoClick()
        {
            ExecuteEvents.Execute<IInfoMessageTarget>(this.transform.parent.gameObject, null, (x, y) => x.RemoveInfo(this));
        }
        
        public class Factory : PlaceholderFactory<DefaultInfoButton>
        {
        }
    }
}
