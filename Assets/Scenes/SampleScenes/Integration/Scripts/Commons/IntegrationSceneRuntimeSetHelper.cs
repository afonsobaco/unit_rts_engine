using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;
using RTSEngine.RTSSelection;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.Integration.Scene
{
    // TODO Remove that?
    public class IntegrationSceneRuntimeSetHelper : MonoBehaviour
    {
        [Inject] private IRuntimeSet<ISelectable> selectables;
        [Inject] private Selection selection;
        // [Inject] private UserInterface userInterface;
        [SerializeField] private List<DefaultSelectable> runtimeSet;
        [SerializeField] private List<DefaultSelectable> actualSelection;
        [SerializeField] private List<DefaultSelectable> uiActualSelection;

        private void Update()
        {
            runtimeSet = selectables.GetMainList().Where(x => x is DefaultSelectable).Select(x => x as DefaultSelectable).ToList();
            actualSelection = selection.GetActualSelection().Where(x => x is DefaultSelectable).Select(x => x as DefaultSelectable).ToList();
            // uiActualSelection = userInterface.GetActualSelection().Where(x => x is DefaultSelectable).Select(x => x as DefaultSelectable).ToList();
        }
    }

}