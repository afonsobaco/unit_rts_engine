using UnityEngine;

namespace RTSEngine.RTSUserInterface.Utils
{
    public static class UserInterfaceUtils
    {
        public static void ClearPanel(RectTransform panel)
        {
            if (panel)
            {
                foreach (Transform child in panel)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

    }
}