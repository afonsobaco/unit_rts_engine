using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.RTSUserInterface.Utils;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{

    public class UserInterfaceContainer : MonoBehaviour
    {
        [SerializeField] private string lookupId;
        [SerializeField] private RectTransform panel;

        public string LookupId { get => lookupId; set => lookupId = value; }
        public RectTransform Panel { get => panel; set => panel = value; }

        public void Clear()
        {
            UserInterfaceUtils.ClearPanel(Panel);
        }

        public void AddToCanvas()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas)
            {
                var placeholder = canvas.GetComponentsInChildren<ContainerPlaceholder>().First(x => x.LookupId.Equals(this.LookupId));
                if (placeholder)
                {
                    this.gameObject.transform.SetParent(placeholder.transform, false);
                }
                else
                {
                    this.gameObject.transform.SetParent(canvas.transform, false);
                }
            }
        }
    }
}
