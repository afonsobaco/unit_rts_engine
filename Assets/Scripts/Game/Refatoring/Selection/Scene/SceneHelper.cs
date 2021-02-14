using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Refactoring;

/**
*
* This class is just for tests
* It should be used only as a reference for real implementation
*
*/
public class SceneHelper : MonoBehaviour
{
    public GameObject prefab;

    [Inject] private Selection selection;
    [Inject] private SignalBus signalBus;

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
                SelectableObjectSelection item = hit.collider.GetComponent<SelectableObjectSelection>();
                if (item != null)
                {
                    GameObject.Destroy((item as SelectableObjectSelection).gameObject);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            _starScreenPoint = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            signalBus.Fire(new AreaSelectionSignal() { StartPoint = _starScreenPoint, EndPoint = Input.mousePosition });
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            signalBus.Fire(new GroupSelectionSignal() { GroupId = 1, CreateNew = Input.GetKey(KeyCode.Z) });
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            var item = GetItem();
            if (item != null)
                signalBus.Fire(new IndividualSelectionSignal() { BlockAreaSelection = false, Clicked = item });
        }
    }

    private void CreateSelectableObject(Vector3 position)
    {
        var newObjet = GameObject.Instantiate(prefab);
        newObjet.transform.position = position;
        newObjet.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private RTSEngine.Core.ISelectable GetItem()
    {
        return selection.GetMainList().GetItem(Random.Range(0, selection.GetMainList().GetAllItems().Count));
    }
}
