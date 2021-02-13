using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Refactoring;

public class SceneHelper : MonoBehaviour
{

    public GameObject prefab;

    [Inject] private Selection selection;
    [Inject] private SignalBus signalBus;

    private Vector3 _starScreenPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            var newObjet = GameObject.Instantiate(prefab);
            newObjet.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0, Random.Range(-4.0f, 4.0f));
            newObjet.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            var item = selection.GetMainList().GetItem(Random.Range(0, selection.GetMainList().GetAllItems().Count));
            if (item != null)
                GameObject.Destroy((item as SelectableObjectSelection).gameObject);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _starScreenPoint = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            signalBus.Fire(new AreaSelectionSignal() { StartPoint = _starScreenPoint, EndPoint = Input.mousePosition });
        }
    }

}
