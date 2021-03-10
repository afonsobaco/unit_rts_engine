using UnityEngine;
using RTSEngine.RTSCamera;
using RTSEngine.RTSSelection;
using RTSEngine.RTSUserInterface;
using Zenject;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneHelper : MonoBehaviour
    {
        [SerializeField] private GameObject[] objects;
        private int createIndex;
        private int count;

        [Inject] private IUserInterfaceLogManager _logManager;

        void Update()
        {
            CreateSceneObjects();
        }

        private void CreateSceneObjects()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                createIndex++;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                createIndex--;
            }

            createIndex = Mathf.Clamp(createIndex, 0, objects.Length - 1);

            if (Input.GetMouseButtonUp(1))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    RemoveSelectableObject();
                }
                else
                {
                    CreateSelectableObject(objects[createIndex]);
                }
            }
        }

        private void RemoveSelectableObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var gdo = hit.transform.GetComponent<IntegrationSceneObject>();
                if (gdo)
                {
                    Destroy(gdo.gameObject);
                    _logManager.AddLog("Model at " + gdo.transform.position + " Destroyed");
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
                var a = newObjet.GetComponent<IntegrationSceneObject>();
                if (a)
                {
                    a.Index = count++;
                }
                newObjet.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                _logManager.AddLog("Model created at " + newObjet.transform.position);
            }
        }
    }

}