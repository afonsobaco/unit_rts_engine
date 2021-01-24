using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager;
using NSubstitute;

namespace Tests.Manager
{
    [TestFixture]
    public class CameraManagerTest
    {
        private const float deltaTime = 0.1f;

        private CameraManager manager;
        private UnityEngine.Camera mainCamera;
        private SelectionManager selectionManager;

        private ICameraSettings cameraSettings;

        public static float DeltaTime => deltaTime;

        public CameraManager Manager { get => manager; set => manager = value; }
        public Camera MainCamera { get => mainCamera; set => mainCamera = value; }
        public SelectionManager SelectionManager { get => selectionManager; set => selectionManager = value; }
        public ICameraSettings CameraSettings { get => cameraSettings; set => cameraSettings = value; }

        [SetUp]
        public void SetUp()
        {
            SelectionManager = Substitute.For<SelectionManager>();
            CameraSettings = Substitute.For<ICameraSettings>();
            Manager = new CameraManager(SelectionManager);

            MainCamera = UnityEngine.Camera.main;
            MainCamera.transform.position = new Vector3(0, 20, -20);
            MainCamera.transform.eulerAngles = new Vector3(45f, 0f, 0f);

            Manager.CameraSettings = CameraSettings;

            CameraSettings.SizeFromMidPoint = 15f;
            CameraSettings.BoundriesOffset = 0.03f;
            CameraSettings.AxisPressure = 0.1f;
            CameraSettings.CameraSpeed = 1f;
            CameraSettings.PanSpeed = 5f;
            CameraSettings.MinZoom = 3f;
            CameraSettings.MaxZoom = 30f;
            CameraSettings.ZoomScale = 10f;
        }

        [TestCase(45f, 10f, 10f)]
        [TestCase(45f, 20f, 20f)]
        [TestCase(60f, 10f, 5.77350187f)]
        [TestCase(60f, 20f, 11.5470037f)]
        public void ShouldGetMockedCameraZDistance(float cameraXRotation, float yPos, float expectedZ)
        {
            MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, yPos, MainCamera.transform.position.z);
            MainCamera.transform.eulerAngles = new Vector3(cameraXRotation, MainCamera.transform.eulerAngles.y, MainCamera.transform.eulerAngles.z);
            var result = Manager.GetCameraZDistance(MainCamera);
            Assert.AreEqual(Mathf.Round(expectedZ), Mathf.Round(result));
        }

        [Test]
        public void ShouldNotMoveCameraWhenCenterCameraToSelectionWithEmptySelection()
        {
            SelectionManager.CurrentSelection.Returns(new List<SelectableObject>());
            var result = Manager.DoCameraCentering(MainCamera);
            Assert.AreEqual(MainCamera.transform.position, result);
        }

        [TestCase(45f, 10f, 10f)]
        [TestCase(45f, 20f, 20f)]
        [TestCase(60f, 10f, 5.77350187f)]
        [TestCase(60f, 20f, 11.5470037f)]
        public void ShouldCenterCameraToPositionWhenCenterCameraToSelection(float cameraXRotation, float yPos, float expectedZ)
        {
            List<SelectableObject> currentSelection = GetDefaultCurrentSelection();
            SelectionManager.GetSelectionMainPoint().Returns(currentSelection[0].transform.position);

            MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, yPos, MainCamera.transform.position.z);
            MainCamera.transform.eulerAngles = new Vector3(cameraXRotation, MainCamera.transform.eulerAngles.y, MainCamera.transform.eulerAngles.z);
            var result = Manager.DoCameraCentering(MainCamera);
            var expected = new Vector3(currentSelection[0].transform.position.x, MainCamera.transform.position.y, Mathf.Round((currentSelection[0].transform.position.z - expectedZ)));
            Assert.AreEqual(expected, new Vector3(result.x, result.y, Mathf.Round(result.z)));

        }

        [TestCase(20f, 2.71f)]
        [TestCase(10f, 1.71f)]
        [TestCase(5f, 1.21f)]
        [TestCase(3f, 1.01f)]
        public void ShouldMoveCameraHorizontally(float yPos, float expected)
        {
            var result = Manager.MoveCamera(1f, yPos, DeltaTime);
            Assert.AreEqual(Mathf.Round(expected * 100), Mathf.Round(result * 100));
        }

        [TestCase(0f, 0f, 1f, 0f, 0f)]
        [TestCase(1f, 0f, 1f, 2.708f, 0f)]
        [TestCase(0f, 1f, 1f, 0f, 2.708f)]
        [TestCase(1f, 1f, 1f, 2.708f, 2.708f)]
        public void ShouldDoAxisCameraMovement(float horizontal, float vertical, float speed, float expectedX, float expectedZ)
        {
            CameraSettings.CameraSpeed = speed;
            var result = Manager.DoAxisCameraMovement(horizontal, vertical, DeltaTime, MainCamera);
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
            CameraSettings.SizeFromMidPoint = size;
            MainCamera.transform.position = new Vector3(camperaPosX, 20f, camperaPosZ);
            var result = Manager.ClampCameraPos(MainCamera);
            Vector3 expected = new Vector3(Mathf.Round(expectedX * 1000), Mathf.Round(20f * 1000), Mathf.Round(expectedZ * 1000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 1000), Mathf.Round(result.y * 1000), Mathf.Round(result.z * 1000));
            Assert.AreEqual(expected, roundedResult);
        }

        [Test]
        public void ShouldNotDoCameraInputMovement()
        {
            Vector3 mousePos = MainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            var result = Manager.DoCameraInputMovement(0f, 0f, mousePos, DeltaTime, MainCamera);
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
            Vector3 mousePos = MainCamera.ViewportToScreenPoint(new Vector2(mouseX, mouseY));
            var result = Manager.DoCameraInputMovement(0f, 0f, mousePos, DeltaTime, MainCamera);
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
            Vector3 mousePos = MainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            var result = Manager.DoCameraInputMovement(horizontal, vertical, mousePos, DeltaTime, MainCamera);
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
            var result = Manager.DoCameraPanning(new Vector2(mouseXAxis, mouseYAxis), DeltaTime, MainCamera);
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
            CameraSettings.MinZoom = minZoom;
            CameraSettings.MaxZoom = maxZoom;
            var result = Manager.DoCameraZooming(deltaScroll, DeltaTime, MainCamera);
            Vector3 expected = new Vector3(0f, Mathf.Round(expectedY * 10000), Mathf.Round(expectedZ * 10000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
            Assert.AreEqual(expected, roundedResult);
        }

        #region methods

        public static SelectableObject CreateGameObject()
        {
            var go = new GameObject();
            var so = go.AddComponent<SelectableObject>();
            return so;
        }

        private List<SelectableObject> GetDefaultCurrentSelection()
        {
            List<SelectableObject> currentSelection = new List<SelectableObject>();
            SelectableObject obj = CreateGameObject();
            obj.transform.position = new Vector3(2, 0, 20);
            currentSelection.Add(obj);

            SelectionManager.CurrentSelection.Returns(currentSelection);
            return currentSelection;
        }

        #endregion

    }
}
