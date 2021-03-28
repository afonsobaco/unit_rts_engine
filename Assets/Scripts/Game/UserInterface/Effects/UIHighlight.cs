using System.Collections;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIHighlight : UIMouseOver
    {
        [SerializeField] private GameObject _highlight;

        public override IEnumerator DoBlurAnimation()
        {
            _highlight.SetActive(MouseOver);
            yield return null;
        }

        public override IEnumerator DoFocusAnimation()
        {
            _highlight.SetActive(MouseOver);
            yield return null;
        }
    }
}

