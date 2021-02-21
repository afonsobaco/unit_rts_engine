using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;

/**
*
* This class is just for tests
* It should be used only as a reference for real implementation
*
*/
public class SceneHelper : MonoBehaviour
{
    public GameObject prefab;

    [Inject] private SignalBus _signalBus;
    [Inject] private IRuntimeSet<ISelectable> _mainList;

    private Vector3 _starScreenPoint;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                CreateSelectableObject(hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                DefaultObject item = hit.collider.GetComponent<DefaultObject>();
                if (item != null)
                {
                    GameObject.Destroy(item.gameObject);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            _starScreenPoint = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _signalBus.Fire(new AreaSelectionSignal() { StartPoint = _starScreenPoint, EndPoint = Input.mousePosition });
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _signalBus.Fire(new GroupSelectionSignal() { GroupId = 1, CreateNew = Input.GetKey(KeyCode.Z) });
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            var item = GetItem();
            if (item != null)
                _signalBus.Fire(new IndividualSelectionSignal() { BlockAreaSelection = false, Clicked = item });
        }
    }

    private void CreateSelectableObject(Vector3 position)
    {
        var newObjet = GameObject.Instantiate(prefab);
        newObjet.transform.position = position;
        int rnd = Random.Range(0, 3);
        DefaultObject selectable = rnd < 2 ? newObjet.AddComponent<GroupableObject>() : newObjet.AddComponent<DefaultObject>();
        selectable.selectionOrder = rnd;
        selectable.objectType = rnd.ToString();
        Color color = new Color();

        switch (rnd)
        {
            case 0:
                color = Color.red;
                break;
            case 1:
                color = Color.green;
                break;
            case 2:
                color = Color.blue;
                break;
            default:
                color = Color.gray;
                break;
        }

        newObjet.GetComponent<Renderer>().material.color = color;// Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private RTSEngine.Core.ISelectable GetItem()
    {
        return _mainList.GetItem(Random.Range(0, _mainList.GetAllItems().Count));
    }
}
