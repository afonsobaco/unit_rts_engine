using System;
using Zenject;
using NUnit.Framework;
using RTSEngine.Manager;

namespace RTSEngine.Player.Test
{
    [TestFixture]
    public class PlayerInputManagerTest : ZenjectUnitTestFixture
    {

        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<ISelectionManager>().To<SelectionManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();
        }

        [Test]
        public void TestInitialValues()
        {
            var manager = Container.Resolve<PlayerInputManager>();
            manager.DebugClass();
        }


    }
}
