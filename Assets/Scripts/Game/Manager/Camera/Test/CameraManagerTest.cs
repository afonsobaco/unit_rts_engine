using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Selection.Mod;
using RTSEngine.Core;
using RTSEngine.Selection;
using NSubstitute;

namespace RTSEngine.Manager.Camera.Tests
{
    [TestFixture]
    public class CameraManagerTest
    {
        // A Test behaves as an ordinary method
        private ICameraManager manager;

        [SetUp]
        public void SetUp()
        {
            ISelectionManager<SelectableObject> selectionManager = Substitute.For<ISelectionManager<SelectableObject>>();
            manager = new CameraManager(selectionManager);
        }


        [Test]
        public void ShouldDoCameraPanning()
        {
            manager.DoCameraPanning(Vector3.zero);
        }


    }
}
