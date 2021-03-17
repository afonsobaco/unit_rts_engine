using UnityEngine;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

public class DefaultUserInterfaceInput : MonoBehaviour
{
    [SerializeField] private KeyCode _toRemoveKeyCode = KeyCode.LeftShift;
    [SerializeField] private KeyCode _changeSubGroupKeyCode = KeyCode.Tab;
    [SerializeField] private KeyCode _centerCameraKeyCode = KeyCode.Space;
    [SerializeField] private KeyCode _partyKeyCode = KeyCode.Z;
    private GameSignalBus _signalBus;
    private UserInterface _userInterface;

    [Inject]
    public void Construct(GameSignalBus signalBus, UserInterface userInterface)
    {
        this._signalBus = signalBus;
        this._userInterface = userInterface;
    }

    private void Update()
    {
        GetChangeSubGroupInput();
        GetCenterCameraInput();
        GetPartyInput();
        GetOtherInputs();
    }

    public virtual void GetPartyInput()
    {
        int partyKeyPressed = GameUtils.GetAnyPartyKeyPressed();
        if (partyKeyPressed > 0)
        {
            if (Input.GetKey(_partyKeyCode))
            {
                _signalBus.Fire(new PartyUpdateSignal() { PartyId = partyKeyPressed });
            }
            else
            {
                _signalBus.Fire(new BannerClickedSignal() { PartyId = partyKeyPressed });
            }
        }
    }

    public virtual void GetCenterCameraInput()
    {
        if (Input.GetKey(_centerCameraKeyCode) && _userInterface.Highlighted != null)
        {
            _signalBus.Fire(new CameraGoToPositionSignal() { Position = _userInterface.Highlighted.Position });
        }
    }

    public virtual void GetChangeSubGroupInput()
    {
        if (Input.GetKeyDown(_changeSubGroupKeyCode))
        {
            _signalBus.Fire(new AlternateSubGroupSignal() { Previous = Input.GetKey(_toRemoveKeyCode) });
            UpdateAll();
        }
    }

    public virtual void UpdateAll()
    {
    }
    public virtual void GetOtherInputs()
    {
    }

}
