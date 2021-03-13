using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{

    public class UserInterfaceContentComponent : MonoBehaviour
    {
        [SerializeField] private string contentId;
        private UserInterfaceContent content;
        public string ContentId { get => contentId; }
        public UserInterfaceContent Content { get => content; set => content = value; }
    }
}
