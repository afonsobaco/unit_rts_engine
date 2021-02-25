using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

public class BannerButton : ZenAutoInjecter
{
    private SignalBus _signalBus;
    public object PartyId { get; set; }

    private Text text;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this._signalBus = signalBus;
    }

    private void Start()
    {
        text = this.GetComponent<Button>().GetComponentInChildren<Text>();
        text.text = PartyId.ToString();
    }

    public void Clicked()
    {
        _signalBus.Fire(new BannerClickedSignal()
        {
            PartyId = this.PartyId,
            ToRemove = Input.GetKey(KeyCode.LeftShift)
        });
    }
}




