using System.Collections;
using Zenject;
using UnityEngine;
namespace RTSEngine.Manager
{

    public class UnitTestScript : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            this._signalBus = signalBus;
        }

        void OnDisable()
        {
            _signalBus.Fire(new SignalA() { Selectable = this });
        }

        void OnEnable()
        {
            _signalBus.Fire(new SignalB() { Selectable = this });
        }
    }
}
