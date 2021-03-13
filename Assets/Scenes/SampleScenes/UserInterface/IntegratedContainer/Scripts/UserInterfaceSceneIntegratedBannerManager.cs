using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneIntegratedBannerManager : UserInterfaceContainerManagerComponent
    {
        [SerializeField] public int maxSize;

        public override void AddContent(UserInterfaceContent content)
        {
            if (GetAllContentComponents().Count <= maxSize)
            {
                base.AddContent(content);
            }
        }
    }
}
