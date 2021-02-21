using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

public class BannerButton : ZenAutoInjecter
{
    private SignalBus _signalBus;
    public object GroupId { get; set; }

    private Text text;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this._signalBus = signalBus;
    }

    private void Start()
    {
        text = this.GetComponent<Button>().GetComponentInChildren<Text>();
        text.text = GroupId.ToString();
    }

    public void Clicked()
    {
        _signalBus.Fire(new BannerClickedSignal()
        {
            GroupId = this.GroupId,
            ToRemove = Input.GetKey(KeyCode.LeftShift)
        });
    }
}




