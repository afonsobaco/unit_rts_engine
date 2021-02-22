using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public abstract class DefaultClickableButton : MonoBehaviour, UIClickable
    {
        private GameSignalBus _signalBus;
        protected GameSignalBus SignalBus { get => _signalBus; set => _signalBus = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }
        public abstract void Clicked();

    }
}
