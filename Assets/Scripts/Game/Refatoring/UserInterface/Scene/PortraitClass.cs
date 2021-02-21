using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;
using UnityEngine;

public class PortraitClass : ISelectable
{

    private string name;
    public int Index { get; set; }
    public bool IsSelected { get; set; }
    public bool IsPreSelected { get; set; }
    public Vector3 Position { get; set; }
    public string Name { get => name; set => name = value; }
    public bool IsHighlighted { get; set; }

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this._signalBus = signalBus;
    }

    public int CompareTo(object obj)
    {
        return 0;
    }
}