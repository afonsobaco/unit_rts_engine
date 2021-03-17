using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using RTSEngine.Utils;

namespace RTSEngine.RTSUserInterface
{
    public interface IUIEffect
    {
        IEnumerator Create();
        IEnumerator Destroy();
        IEnumerator Show();
        IEnumerator Hide();
        IEnumerator OnMouseOver();
        IEnumerator OnClick();
    }
}

