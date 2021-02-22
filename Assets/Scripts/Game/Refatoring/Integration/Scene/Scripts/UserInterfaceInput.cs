using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class UserInterfaceInput : DefaultUserInterfaceInput
    {
        private DefaultMiniatureButton.Factory _miniatureFactory;

        [Inject]
        public void Construct(DefaultMiniatureButton.Factory miniatureFactory)
        {
            this._miniatureFactory = miniatureFactory;
        }

        public override void UpdateAll()
        {
        }
        public override void GetOtherInputs()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _miniatureFactory.Create();
            }
        }
    }

}