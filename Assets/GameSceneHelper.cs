using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Manager;
public class GameSceneHelper : MonoBehaviour
{
    private SelectionManager _manager;

    [Inject]
    public void Construct(SelectionManager manager)
    {
        this._manager = manager;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var a = this._manager.GetCurrentSelection();
            if (a.Count > 0)
            {
                Debug.Log("Damage!");
                a.ToList().ForEach(x => x.LifeStatus.CurrentValue -= 5);
            }
        }
    }
}
