using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

public class PortraitButton : ZenAutoInjecter
{

    public PortraitClass Selectable { get; set; }

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this._signalBus = signalBus;
    }

    private void Start()
    {
        if (Selectable != null)
        {
            this.GetComponent<Button>().GetComponentInChildren<Text>().text = Selectable.Name;
        }
    }

    public void Clicked()
    {
        _signalBus.Fire(new PortraitClickedSignal() { Selection = Selectable });
    }
}