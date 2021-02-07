using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUITesterHelper : MonoBehaviour
    {
        [SerializeField] private SelectableObjectBehaviour[] _prefabs;
        private List<SelectableObjectBehaviour> _selection = new List<SelectableObjectBehaviour>();

        private Dictionary<KeyCode, int> _keys = new Dictionary<KeyCode, int>()
            {
                {KeyCode.Keypad1, 1},
                {KeyCode.Keypad2, 2},
                {KeyCode.Keypad3, 3},
                {KeyCode.Keypad4, 4},
                {KeyCode.Keypad5, 5},
                {KeyCode.Keypad6, 6},
                {KeyCode.Keypad7, 7},
                {KeyCode.Keypad8, 8},
                {KeyCode.Keypad9, 9},
                {KeyCode.Keypad0, 10}
            };

        private SignalBus _signalBus;

        private GUIManager _manager;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            this._signalBus = signalBus;
        }

        private void Update()
        {
            int keyPressed = GetAnyGroupKeyPressed();
            if (keyPressed > 0 && keyPressed - 1 < _prefabs.Length)
            {
                if (Input.GetKey(KeyCode.X))
                {
                    var toRemove = _selection.Find(x => x.IsCompatible(_prefabs[keyPressed - 1]));
                    if (toRemove)
                        _selection.Remove(toRemove);
                }
                else
                {
                    _selection.Add(_prefabs[keyPressed - 1]);
                }
                UpdateScene();
            }
        }

        public int GetAnyGroupKeyPressed()
        {
            foreach (KeyValuePair<KeyCode, int> entry in this._keys)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    return entry.Value;
                }
            }
            return 0;
        }

        public void UpdateScene()
        {
            if (this._signalBus != null)
            {
                this._signalBus.Fire(new SelectionChangeSignal() { Selection = _selection.ToArray() });
            }
        }
    }

}
