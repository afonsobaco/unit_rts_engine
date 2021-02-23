using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHelper : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] [Range(0, 100)] private int index;

    void Update()
    {
        index = Mathf.Clamp(index, 0, objects.Length - 1);
        CreateSceneObjects();

    }

    private void CreateSceneObjects()
    {
        if (Input.GetMouseButtonUp(1))
        {
            CreateSelectableObject(objects[index]);
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
