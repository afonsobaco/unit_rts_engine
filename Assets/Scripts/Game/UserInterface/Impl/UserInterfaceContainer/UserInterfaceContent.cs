using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{

    public class UserInterfaceContent
    {
        public string ContentId { get; set; }
        public void UpdateAppearance()
        {
            Debug.Log("Should update appearance");
        }
    }
}
