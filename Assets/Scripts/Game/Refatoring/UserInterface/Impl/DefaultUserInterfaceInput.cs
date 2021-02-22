using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Refactoring;
using RTSEngine.Utils;
using Zenject;
using System;

public class DefaultUserInterfaceInput : MonoBehaviour
{
    [SerializeField] private KeyCode ToRemoveKeyCode = KeyCode.LeftShift;
    [SerializeField] private KeyCode ChangeSubGroupKeyCode = KeyCode.Tab;
    [SerializeField] private KeyCode CenterCameraToSelection = KeyCode.Space;
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
        GetOtherInputs();
    }

    public virtual void GetCenterCameraInput()
    {
        if (Input.GetKey(CenterCameraToSelection) && _userInterface.Highlighted != null)
        {
            _signalBus.Fire(new CameraGoToPositionSignal() { Position = _userInterface.Highlighted.Position });
        }
    }

    public virtual void GetChangeSubGroupInput()
    {
        if (Input.GetKeyDown(ChangeSubGroupKeyCode))
        {
            _signalBus.Fire(new AlternateSubGroupSignal() { Previous = Input.GetKey(ToRemoveKeyCode) });
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
