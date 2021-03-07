using UnityEngine.UI;
using RTSEngine.Signal;
using UnityEngine;
using UnityEngine.UIElements;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneCommandButton : DefaultInfoButton
    {
        [SerializeField] private GameObject highlight;

        public override void DoMouseEnter()
        {
            if (highlight)
            {
                highlight.SetActive(true);
            }
        }

        public override void DoMouseExit()
        {
            if (highlight)
            {
                highlight.SetActive(false);
            }
        }

    }

}