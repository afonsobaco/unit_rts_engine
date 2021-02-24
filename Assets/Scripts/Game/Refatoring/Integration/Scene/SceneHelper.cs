using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Refactoring;

public class SceneHelper : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    private int index;

    void Update()
    {
        CreateSceneObjects();

    }

    private void CreateSceneObjects()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            index++;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            index--;
        }

        index = Mathf.Clamp(index, 0, objects.Length - 1);

        if (Input.GetMouseButtonUp(1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                RemoveSelectableObject();
            }
            else
            {
                CreateSelectableObject(objects[index]);
            }
        }
    }

    private void RemoveSelectableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var gdo = hit.transform.GetComponent<GameDefaultObject>();
            if (gdo)
            {
                Destroy(gdo.gameObject);
            }
        }
    }

    private void CreateSelectableObject(GameObject prefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var newObjet = GameObject.Instantiate(prefab);
            newObjet.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
        }
    }
}
