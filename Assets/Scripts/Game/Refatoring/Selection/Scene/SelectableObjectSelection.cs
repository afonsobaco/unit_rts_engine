using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

public class SelectableObjectSelection : DefaultSelectable, IGroupable
{
    private SignalBus _signalBus;
    public int selectionOrder;
    public string objectType;

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

    private void OnMouseUpAsButton()
    {
        _signalBus.Fire(new IndividualSelectionSignal() { Clicked = this, BlockAreaSelection = true });
    }

    public override int CompareTo(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return -1;
        }

        var other = obj as SelectableObjectSelection;

        int v = other.selectionOrder - this.selectionOrder;

        return v;
    }

    public bool IsCompatible(object other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }
        return this.objectType.Equals((other as SelectableObjectSelection).objectType);
    }

}
