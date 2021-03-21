using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Signal;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneMiniatureButton : MonoBehaviour// DefaultMiniatureButton
    {
        // [SerializeField] private Image picture;
        // [SerializeField] private Image highlight;
        // [SerializeField] private Image LifeBar;
        // [SerializeField] private Image ManaBar;

        // public override void UpdateApperance()
        // {
        //     if (ObjectReference is IntegrationSceneObject)
        //     {
        //         var obj = ObjectReference as IntegrationSceneObject;
        //         picture.sprite = obj.Picture;
        //         highlight.gameObject.SetActive(obj.IsHighlighted);
        //     }
        // }

        // public override void DoClick()
        // {
        //     if (ObjectReference is IntegrationSceneObject)
        //         SignalBus.Fire(new MiniatureClickedSignal() { Selected = ObjectReference as IntegrationSceneObject });
        // }
    }
}
