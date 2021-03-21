using System;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.Signal;
using UnityEngine;
using Zenject;

namespace RTSEngine.Utils
{

    [Obsolete]
    public class GameSignalBus
    {
        private SignalBus _signalBus;

        public GameSignalBus(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public virtual void Fire(object signal)
        {
            _signalBus.Fire(signal);
        }
    }
}
