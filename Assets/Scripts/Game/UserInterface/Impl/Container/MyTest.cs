using UnityEngine;
using Zenject;
namespace RTSEngine.RTSUserInterface
{
    public class MyTest : MonoBehaviour, IMyTest, IInitializable
    {
        public void DoSomenthing()
        {
            Debug.Log("DoSomenthing");
        }

        public void Initialize()
        {
            Debug.Log("Initialized");
        }
    }

    internal interface IMyTest
    {
        void DoSomenthing();
    }
}