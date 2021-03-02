using UnityEngine;
using UnityEngine.UI;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;


namespace RTSEngine.Refactoring.Scene.Selection
{
    /**
    *
    * This class is just for tests
    * It should be used only as a reference for real implementation
    *
    */

    public class SceneHelper : MonoBehaviour
    {
        public GameObject prefab;
        public RectTransform _rectSelectionBox;
        public SelectionBox _selectionBox;
        private SignalBus _signalBus;
        private IRuntimeSet<ISelectable> _mainList;
        private Vector3 _starScreenPoint;
        private int createIndex = 1;
        private int strengthIndex = 0;


        [Inject]
        public void Construct(SignalBus signalBus, IRuntimeSet<ISelectable> mainList)
        {
            _signalBus = signalBus;
            _mainList = mainList;
        }

        private void Start()
        {
            _selectionBox = new SelectionBox(_rectSelectionBox);
        }

        void Update()
        {
            ChangeColor();
            ChangeStrengthColor();
            AddDeleteCube();
            SelectionWithMouse();
            AddRemoveBanner();
        }


        private void ChangeStrengthColor()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                strengthIndex++;
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                strengthIndex--;
            }
            strengthIndex = Mathf.Clamp(strengthIndex, 0, 2);
        }

        private void ChangeColor()
        {
            var numpadKey = GameUtils.GetAnyNumpadKeyPressed();
            if (numpadKey > 0 && numpadKey <= 3)
            {
                createIndex = numpadKey;
            }
        }

        private void AddDeleteCube()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        DestroySelectableObject(hit.collider);
                    }
                    else
                    {
                        CreateSelectableObject(hit.point);
                    }
                }
            }
        }

        private void DestroySelectableObject(Collider hit)
        {
            SelectionSceneObject item = hit.GetComponent<SelectionSceneObject>();
            if (item != null)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        private void SelectionWithMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _starScreenPoint = Input.mousePosition;
                _selectionBox.Activate(_starScreenPoint);
            }
            _selectionBox.DrawSelectionBox(Input.mousePosition);
            if (Input.GetMouseButtonUp(0))
            {
                _selectionBox.Deactivate();
                _signalBus.Fire(new AreaSelectionSignal() { StartPoint = _starScreenPoint, EndPoint = Input.mousePosition });
            }
        }

        private void AddRemoveBanner()
        {
            var keyPressed = GameUtils.GetAnyPartyKeyPressed();
            if (keyPressed > 0)
            {
                _signalBus.Fire(new PartySelectionSignal() { PartyId = keyPressed, CreateNew = Input.GetKey(KeyCode.Z) });
            }
        }

        private void CreateSelectableObject(Vector3 position)
        {
            var obj = GameObject.Instantiate(prefab);
            obj.transform.position = new Vector3(position.x, 0.5f, position.z);
            SelectionSceneObject selectable = obj.GetComponent<SelectionSceneObject>();
            switch (createIndex)
            {
                case 1:
                    CreateACube(Color.red, "Human", "Unit", obj);
                    break;
                case 2:
                    CreateACube(Color.green, "Farm", "Building", obj);
                    break;
                case 3:
                    CreateACube(Color.blue, "Mine", "Consumable", obj);
                    break;
                default:
                    CreateACube(Color.magenta, "Rock", "Environment", obj);
                    break;
            }
        }

        private void CreateACube(Color color, string subGroupName, string typeName, GameObject obj)
        {
            SelectionSceneGameSubGroup subGroup = obj.AddComponent<SelectionSceneGameSubGroup>();
            SelectionSceneGameType type = obj.AddComponent<SelectionSceneGameType>();
            type.Type = typeName;

            subGroup.SubGroup = subGroupName + " " + strengthIndex;
            subGroup.Priority = strengthIndex;

            obj.GetComponent<Renderer>().material.color = GetColor(color);
            obj.transform.localScale = GetScale();
        }

        private Vector3 GetScale()
        {
            return new Vector3(0.75f + GetMultiplier() / 2, 0.75f + GetMultiplier() / 2, 0.75f + GetMultiplier() / 2);
        }

        private Color GetColor(Color color)
        {
            Color newColor = new Color(color.r * GetMultiplier(), color.g * GetMultiplier(), color.b * GetMultiplier());
            return newColor;
        }
        private float GetMultiplier()
        {
            return ((strengthIndex + 1f) / 2f);
        }
    }
}