using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RTSEngine.Core;
using RTSEngine.Manager;
using NSubstitute;

namespace Tests
{
    public class GUIManagerTest
    {

        private GUIManager manager;

        [SetUp]
        public void SetUp()
        {
            manager = Substitute.ForPartsOf<GUIManager>();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ShouldUpdateGUIOnSelection()
        {

        }

    }
}
