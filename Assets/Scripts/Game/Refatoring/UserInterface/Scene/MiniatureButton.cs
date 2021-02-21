using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

public class MiniatureButton : ZenAutoInjecter
{
    private SignalBus _signalBus;
    public MiniatureClass Selectable { get; set; }

    private Text text;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this._signalBus = signalBus;
    }

    private void Start()
    {
        text = this.GetComponent<Button>().GetComponentInChildren<Text>();
        text.text = Selectable.Type + " " + Selectable.Index;
    }

    private void Update()
    {
        if (Selectable.IsHighlighted)
        {
            text.fontSize = (int)Mathf.PingPong(Time.time * 15, 4) + 10;
        }
    }

    public void Clicked()
    {
        _signalBus.Fire(new MiniatureClickedSignal()
        {
            Selected = this.Selectable,
            AsGroup = Input.GetKey(KeyCode.LeftControl),
            ToRemove = Input.GetKey(KeyCode.LeftShift)
        });
    }
}




