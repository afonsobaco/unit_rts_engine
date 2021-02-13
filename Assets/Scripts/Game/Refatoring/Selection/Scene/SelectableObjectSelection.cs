using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

public class SelectableObjectSelection : DefaultSelectable
{
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnEnable()
    {
        _signalBus.Fire(new SelectableObjectCreatedSignal { Selectable = this });
    }

    private void OnDisable()
    {
        _signalBus.Fire(new SelectableObjectDeletedSignal { Selectable = this });
    }

    private void Update()
    {
        if (this.IsSelected)
        {
            this.transform.localScale = new Vector3(Mathf.PingPong(Time.time * 0.2f, 0.1f) + 1, Mathf.PingPong(Time.time * 0.2f, 0.1f) + 1, Mathf.PingPong(Time.time * 0.2f, 0.1f) + 1);
        }
    }
}
