using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Manager;
using NSubstitute;

namespace Tests.Manager
{
    [TestFixture]
    public class CameraManagerTest
    {
        private const float deltaTime = 0.1f;

        private ICameraManager manager;
        private UnityEngine.Camera mainCamera;
        private ISelectionManager<ISelectableObjectBehaviour, IBaseSelectionMod, SelectionTypeEnum> selectionManager;

        private ICameraSettings cameraSettings;

        public static float DeltaTime => deltaTime;


        [SetUp]
        public void SetUp()
        {
            selectionManager = Substitute.For<ISelectionManager<ISelectableObjectBehaviour, IBaseSelectionMod, SelectionTypeEnum>>();
            cameraSettings = Substitute.For<ICameraSettings>();
            manager = Substitute.ForPartsOf<CameraManager>(new object[] { selectionManager });

            mainCamera = UnityEngine.Camera.main;
            mainCamera.transform.position = new Vector3(0, 20, -20);
            mainCamera.transform.eulerAngles = new Vector3(45f, 0f, 0f);

            manager.CameraSettings = cameraSettings;

            cameraSettings.SizeFromMidPoint = 15f;
            cameraSettings.BoundriesOffset = 0.03f;
            cameraSettings.AxisPressure = 0.1f;
            cameraSettings.CameraSpeed = 1f;
            cameraSettings.PanSpeed = 5f;
            cameraSettings.MinZoom = 3f;
            cameraSettings.MaxZoom = 30f;
            cameraSettings.ZoomScale = 10f;
        }

        [TestCase(45f, 10f, 10f)]
        [TestCase(45f, 20f, 20f)]
        [TestCase(60f, 10f, 5.77350187f)]
        [TestCase(60f, 20f, 11.5470037f)]
        public void ShouldGetMockedCameraZDistance(float cameraXRotation, float yPos, float expectedZ)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, yPos, mainCamera.transform.position.z);
            mainCamera.transform.eulerAngles = new Vector3(cameraXRotation, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
            var result = manager.GetCameraZDistance(mainCamera);
            Assert.AreEqual(Mathf.Round(expectedZ), Mathf.Round(result));
        }

        [Test]
        public void ShouldNotMoveCameraWhenCenterCameraToSelectionWithEmptySelection()
        {
            selectionManager.GetCurrentSelection().Returns(new HashSet<ISelectableObjectBehaviour>());
            var result = manager.DoCameraCentering(mainCamera);
            Assert.AreEqual(mainCamera.transform.position, result);
        }

        [TestCase(45f, 10f, 10f)]
        [TestCase(45f, 20f, 20f)]
        [TestCase(60f, 10f, 5.77350187f)]
        [TestCase(60f, 20f, 11.5470037f)]
        public void ShouldCenterCameraToPositionWhenCenterCameraToSelection(float cameraXRotation, float yPos, float expectedZ)
        {

            Vector3 expectedPos = new Vector3(2, 0, 20);
            HashSet<ISelectableObjectBehaviour> list = new HashSet<ISelectableObjectBehaviour>();
            list.Add(Substitute.For<ISelectableObjectBehaviour>());
            list.First().Position.Returns(expectedPos);
            selectionManager.GetCurrentSelection().Returns(list);

            //TODO adjust test
            // selectionManager.GetSelectionMainPoint().Returns(expectedPos);



            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, yPos, mainCamera.transform.position.z);
            mainCamera.transform.eulerAngles = new Vector3(cameraXRotation, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
            var result = manager.DoCameraCentering(mainCamera);
            var expected = new Vector3(expectedPos.x, mainCamera.transform.position.y, Mathf.Round((expectedPos.z - expectedZ)));
            Assert.AreEqual(expected, new Vector3(result.x, result.y, Mathf.Round(result.z)));

        }

        [TestCase(20f, 2.71f)]
        [TestCase(10f, 1.71f)]
        [TestCase(5f, 1.21f)]
        [TestCase(3f, 1.01f)]
        public void ShouldMoveCameraHorizontally(float yPos, float expected)
        {
            var result = manager.MoveCamera(1f, yPos, DeltaTime);
            Assert.AreEqual(Mathf.Round(expected * 100), Mathf.Round(result * 100));
        }

        [TestCase(0f, 0f, 1f, 0f, 0f)]
        [TestCase(1f, 0f, 1f, 2.708f, 0f)]
        [TestCase(0f, 1f, 1f, 0f, 2.708f)]
        [TestCase(1f, 1f, 1f, 2.708f, 2.708f)]
        public void ShouldDoAxisCameraMovement(float horizontal, float vertical, float speed, float expectedX, float expectedZ)
        {
            cameraSettings.CameraSpeed = speed;
            var result = manager.DoAxisCameraMovement(horizontal, vertical, DeltaTime, mainCamera);
            Vector3 expected = new Vector3(Mathf.Round(expectedX * 1000), 0f, Mathf.Round(expectedZ * 1000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 1000), Mathf.Round(result.y * 1000), Mathf.Round(result.z * 1000));
            Assert.AreEqual(expected, roundedResult);

        }

        [TestCase(50f, 60f, -20f, 30f, -20f)]
        [TestCase(50f, -60f, -20f, -30f, -20f)]
        [TestCase(50f, 0f, 80f, 0f, 30f)]
        [TestCase(50f, 0f, -80f, 0f, -60f)]
        [TestCase(10f, 60f, -20f, 10f, -10f)]
        [TestCase(10f, -60f, -20f, -10f, -10f)]
        [TestCase(10f, 0f, 80f, 0f, 10f)]
        [TestCase(10f, 0f, -80f, 0f, -10f)]
        public void ShouldClampCameraPos(float size, float camperaPosX, float camperaPosZ, float expectedX, float expectedZ)
        {
            cameraSettings.SizeFromMidPoint = size;
            mainCamera.transform.position = new Vector3(camperaPosX, 20f, camperaPosZ);
            var result = manager.ClampCameraPos(mainCamera);
            Vector3 expected = new Vector3(Mathf.Round(expectedX * 1000), Mathf.Round(20f * 1000), Mathf.Round(expectedZ * 1000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 1000), Mathf.Round(result.y * 1000), Mathf.Round(result.z * 1000));
            Assert.AreEqual(expected, roundedResult);
        }

        [Test]
        public void ShouldNotDoCameraInputMovementWhenNotInBoundries()
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            var result = manager.DoCameraInputMovement(0f, 0f, mousePos, DeltaTime, mainCamera);
            Assert.AreEqual(Vector3.zero, result);
        }

        [Test]
        public void ShouldNotDoCameraInputMovementWhenIsSelecting()
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(1f, 1f));
            selectionManager.IsSelecting().Returns(true);
            var result = manager.DoCameraInputMovement(0f, 0f, mousePos, DeltaTime, mainCamera);
            Assert.AreEqual(Vector3.zero, result);
        }


        [TestCase(0f, 0f, -2.708f, -2.708f)]
        [TestCase(0f, 1f, -2.708f, 2.708f)]
        [TestCase(1f, 0f, 2.708f, -2.708f)]
        [TestCase(1f, 1f, 2.708f, 2.708f)]
        [TestCase(0.5f, 0f, 0f, -2.708f)]
        [TestCase(0f, 0.5f, -2.708f, 0f)]
        [TestCase(0.5f, 1f, 0f, 2.708f)]
        [TestCase(1f, 0.5f, 2.708f, 0f)]
        [TestCase(2f, 0.5f, 0, 0f)]
        [TestCase(0.5f, 2f, 0, 0f)]
        public void ShouldDoCameraInputMovementWhenMouseOnBoundries(float mouseX, float mouseY, float expectedX, float expectedZ)
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(mouseX, mouseY));
            var result = manager.DoCameraInputMovement(0f, 0f, mousePos, DeltaTime, mainCamera);
            Vector3 expected = new Vector3(Mathf.Round(expectedX * 10000), 0f, Mathf.Round(expectedZ * 10000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
            Assert.AreEqual(expected, roundedResult);
        }

        [TestCase(0f, 0f, 0f, 0f)]
        [TestCase(0f, 1f, 0f, 2.708f)]
        [TestCase(1f, 0f, 2.708f, 0f)]
        [TestCase(1f, 1f, 2.708f, 2.708f)]
        [TestCase(0f, -1f, 0f, -2.708f)]
        [TestCase(-1f, 0f, -2.708f, 0f)]
        [TestCase(-1f, -1f, -2.708f, -2.708f)]
        [TestCase(-1f, 1f, -2.708f, 2.708f)]
        [TestCase(1f, -1f, 2.708f, -2.708f)]
        [TestCase(.02f, 0f, 0f, 0f)]
        [TestCase(0f, .02f, 0f, 0f)]
        public void ShouldDoCameraInputMovementWhenAxisPressed(float horizontal, float vertical, float expectedX, float expectedZ)
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            var result = manager.DoCameraInputMovement(horizontal, vertical, mousePos, DeltaTime, mainCamera);
            Vector3 expected = new Vector3(Mathf.Round(expectedX * 10000), 0f, Mathf.Round(expectedZ * 10000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
            Assert.AreEqual(expected, roundedResult);
        }

        [TestCase(0f, 0f, 0f, 0f, 0f)]
        [TestCase(0f, 1f, 0f, -7.0711f, -7.0711f)]
        [TestCase(1f, 0f, -10f, 0f, 0f)]
        [TestCase(1f, 1f, -10f, -7.0711f, -7.0711f)]
        [TestCase(0f, -1f, 0f, 7.0711f, 7.0711f)]
        [TestCase(-1f, 0f, 10f, 0f, 0f)]
        [TestCase(-1f, -1f, 10f, 7.0711f, 7.0711f)]
        [TestCase(-1f, 1f, 10f, -7.0711f, -7.0711f)]
        [TestCase(1f, -1f, -10f, 7.0711f, 7.0711f)]
        public void ShouldDoCameraPanning(float mouseXAxis, float mouseYAxis, float expectedX, float expectedY, float expectedZ)
        {
            var result = manager.DoCameraPanning(new Vector2(mouseXAxis, mouseYAxis), DeltaTime, mainCamera);
            Vector3 expected = new Vector3(Mathf.Round(expectedX * 10000), Mathf.Round(expectedY * 10000), Mathf.Round(expectedZ * 10000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
            Assert.AreEqual(expected, roundedResult);
        }

        [TestCase(3f, 30f, 0f, 20f, -20f)]
        [TestCase(3f, 30f, 1f, 19.2929f, -19.2929f)]
        [TestCase(3f, 30f, -1f, 20.7071f, -20.7071f)]
        [TestCase(3f, 30f, .5f, 19.6464f, -19.6464f)]
        [TestCase(3f, 30f, -.5f, 20.3536f, -20.3536f)]
        [TestCase(3f, 30f, .2f, 19.8586f, -19.8586f)]
        [TestCase(3f, 30f, -.8f, 20.5657f, -20.5657f)]
        [TestCase(19.5f, 20.5f, .8f, 19.5f, -19.5f)]
        [TestCase(19.5f, 20.5f, -.8f, 20.5f, -20.5f)]
        public void ShouldDoCameraZoom(float minZoom, float maxZoom, float deltaScroll, float expectedY, float expectedZ)
        {
            cameraSettings.MinZoom = minZoom;
            cameraSettings.MaxZoom = maxZoom;
            var result = manager.DoCameraZooming(deltaScroll, DeltaTime, mainCamera);
            Vector3 expected = new Vector3(0f, Mathf.Round(expectedY * 10000), Mathf.Round(expectedZ * 10000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
            Assert.AreEqual(expected, roundedResult);
        }

        #region methods

        #endregion

    }
}
