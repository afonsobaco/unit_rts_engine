using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class InjectionTest : ZenAutoInjecter
{
    [Inject]
    private SignalBus _signalBus;

    private void Update()
    {
        Debug.Log(_signalBus);
    }
}
