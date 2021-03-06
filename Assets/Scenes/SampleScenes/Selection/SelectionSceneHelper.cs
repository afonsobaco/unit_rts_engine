using UnityEngine;
using UnityEngine.UI;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;


namespace RTSEngine.RTSSelection.Scene
{
    /**
    *
    * This class is just for tests
    * It should be used only as a reference for real implementation
    *
    */

    public class SelectionSceneHelper : DefaultSelectionInput
    {
        public GameObject prefab;
        public RectTransform _rectSelectionBox;
        public SelectionBox _selectionBox;
        private IRuntimeSet<ISelectable> _mainList;
        private int createIndex = 1;
        private int strengthIndex = 0;


        [Inject]
        public void Construct(IRuntimeSet<ISelectable> mainList)
        {
            _mainList = mainList;
        }

        private void Start()
        {
            _selectionBox = new SelectionBox(_rectSelectionBox);
        }

        public override void GetOtherInputs()
        {
            _selectionBox.DrawSelectionBox(IsSelecting, StartScreenPoint, Input.mousePosition);
            ChangeColor();
            ChangeStrengthColor();
            AddDeleteCube();
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

        private void CreateSelectableObject(Vector3 position)
        {
            switch (createIndex)
            {
                case 1:
                    CreateACube(Color.red, "Human", "Unit", position);
                    break;
                case 2:
                    CreateACube(Color.green, "Farm", "Building", position);
                    break;
                case 3:
                    CreateACube(Color.blue, "Mine", "Consumable", position);
                    break;
                default:
                    CreateACube(Color.magenta, "Rock", "Environment", position);
                    break;
            }
        }

        private void CreateACube(Color color, string subGroupName, string typeName, Vector3 position)
        {
            var obj = GameObject.Instantiate(prefab);

            SelectionSceneGameSubGroup subGroup = obj.AddComponent<SelectionSceneGameSubGroup>();
            subGroup.SubGroup = subGroupName + " " + strengthIndex;
            subGroup.Priority = strengthIndex;

            SelectionSceneGameType type = obj.AddComponent<SelectionSceneGameType>();
            type.Type = typeName;

            obj.GetComponent<SelectionSceneObject>().MainRenderer.material.color = GetColor(color);
            obj.transform.localScale = GetScale();
            obj.transform.position = new Vector3(position.x, obj.transform.localScale.y / 2 + 0.01f, position.z);
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