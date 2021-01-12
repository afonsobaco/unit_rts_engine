using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RTSEngine.Manager;
using RTSEngine.Selection;
using NSubstitute;

namespace Tests
{
    public class SelectionManagerTest
    {

        private SelectionManagerXP manager;
        // A Test behaves as an ordinary method
        [Test]
        public void ShouldGetSelectionTypeCorrectly()
        {
            manager = new SelectionManagerXP();
            var result = manager.GetSelectionType();
            Assert.AreEqual(SelectionTypeEnum.CLICK, result);
            
        }



    }
}
