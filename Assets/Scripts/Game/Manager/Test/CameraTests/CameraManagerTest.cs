using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RTSEngine.Manager.Impls;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Core.Impls;
using NSubstitute;

namespace Tests.Manager
{
    [TestFixture]
    public class CameraManagerTest
    {
        private const float deltaTime = 0.1f;

        private CameraManager manager;
        private UnityEngine.Camera mainCamera;
        private ISelectionManager<SelectableObject> selectionManager;

        private ICameraSettings cameraSettings;

        [SetUp]
        public void SetUp()
        {
            mainCamera = UnityEngine.Camera.main;
            mainCamera.transform.position = new Vector3(0, 20, -20);
            selectionManager = Substitute.For<ISelectionManager<SelectableObject>>();
            cameraSettings = Substitute.For<ICameraSettings>();
            manager = new CameraManager(selectionManager);
            manager.Settings = cameraSettings;
            cameraSettings.SizeFromMidPoint = 15f;
            cameraSettings.BoundriesOffset = 0.03f;
            cameraSettings.AxisPressure = 0.1f;
        }

        [Test]
        public void ShouldGetMockedCameraZDistance()
        {
            var cameraPosMocks = new List<Vector3>()
            {
                new Vector3(45f, 10f, 10f),
                new Vector3(45f, 20f, 20f),
                new Vector3(60f, 10f, 5.77350187f),
                new Vector3(60f, 20f, 11.5470037f),
            };
            foreach (var item in cameraPosMocks)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, item.y, mainCamera.transform.position.z);
                mainCamera.transform.eulerAngles = new Vector3(item.x, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
                var result = manager.GetCameraZDistance(mainCamera);
                Assert.AreEqual(Mathf.Round(item.z), Mathf.Round(result));
            }
        }


        [Test]
        public void ShouldNotMoveCameraWhenCenterCameraToSelectionWithEmptySelection()
        {
            selectionManager.CurrentSelection.Returns(new List<SelectableObject>());
            var result = manager.DoCameraCentering(mainCamera);
            Assert.AreEqual(mainCamera.transform.position, result);
        }



        [Test]
        public void ShouldCenterCameraToPositionWhenCenterCameraToSelection()
        {
            List<SelectableObject> currentSelection = new List<SelectableObject>();
            SelectableObject obj = CreateGameObject<SelectableObject>();
            obj.transform.position = new Vector3(2, 0, 20);
            currentSelection.Add(obj);

            selectionManager.CurrentSelection.Returns(currentSelection);
            selectionManager.GetSelectionMainPoint().Returns(obj.transform.position);

            var cameraPosMocks = new List<Vector3>()
            {
                new Vector3(45f, 10f, 10f),
                new Vector3(45f, 20f, 20f),
                new Vector3(60f, 10f, 5.77350187f),
                new Vector3(60f, 20f, 11.5470037f),
            };

            foreach (var item in cameraPosMocks)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, item.y, mainCamera.transform.position.z);
                mainCamera.transform.eulerAngles = new Vector3(item.x, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
                var result = manager.DoCameraCentering(mainCamera);
                var expected = new Vector3(obj.transform.position.x, mainCamera.transform.position.y, Mathf.Round((obj.transform.position.z - item.z)));
                Assert.AreEqual(expected, new Vector3(result.x, result.y, Mathf.Round(result.z)));
            }
        }

        [Test]
        public void ShouldMoveCameraHorizontally()
        {

            // Vector3(speed, yPos, return)
            var list = new List<Vector3>()
            {
                new Vector3(1f, 20f, 2.71f),
                new Vector3(1f, 10f, 1.71f),
                new Vector3(1f, 5f, 1.21f),
                new Vector3(1f, 3f, 1.01f),
            };

            foreach (var item in list)
            {
                cameraSettings.CameraSpeed = item.x;
                var result = manager.MoveCamera(1f, item.y, deltaTime);
                Assert.AreEqual(Mathf.Round(item.z * 100), Mathf.Round(result * 100));
            }

        }

        [Test]
        public void ShouldDoAxisCameraMovement()
        {

            // Vector3(speed, yPos, return)
            var list = new List<Vector3>()
            {
                new Vector3(0f, 0f, 1f),
                new Vector3(1f, 0f, 1f),
                new Vector3(0f, 1f, 1f),
                new Vector3(1f, 1f, 1f),
            };

            var expectedResults = new List<Vector3>()
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(2.708f, 0f, 0f),
                new Vector3(0f, 0f, 2.708f),
                new Vector3(2.708f, 0f, 2.708f),
            };

            for (int i = 0; i < list.Count; i++)
            {
                Vector3 item = list[i];
                cameraSettings.CameraSpeed = item.z;
                var result = manager.DoAxisCameraMovement(item.x, item.y, deltaTime, mainCamera);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 1000), Mathf.Round(expectedResults[i].y * 1000), Mathf.Round(expectedResults[i].z * 1000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 1000), Mathf.Round(result.y * 1000), Mathf.Round(result.z * 1000));
                Assert.AreEqual(expected, roundedResult);

            }
        }

        [Test]
        public void ShouldClampCameraPos()
        {
            cameraSettings.CameraSpeed = 1f;
            cameraSettings.SizeFromMidPoint = 15f;
            float zDistance = 11.547f; //mocked z distance

            // Vector3(speed, yPos, return)
            float xMovement = 50f;
            float zMovement = 50f;
            var listOfAddition = new List<Vector3>()
            {
                new Vector3(xMovement,mainCamera.transform.position.y, mainCamera.transform.position.z),
                new Vector3(-xMovement,mainCamera.transform.position.y, mainCamera.transform.position.z),
                new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, zMovement),
                new Vector3(mainCamera.transform.position.x,mainCamera.transform.position.y, -zMovement),
            };

            var expectedResults = new List<Vector3>()
            {
                new Vector3(
                    (cameraSettings.SizeFromMidPoint - zDistance),
                    mainCamera.transform.position.y,
                    mainCamera.transform.position.z),

                new Vector3(
                    zDistance - cameraSettings.SizeFromMidPoint,
                    mainCamera.transform.position.y,
                    mainCamera.transform.position.z),

                new Vector3(
                    mainCamera.transform.position.x,
                    mainCamera.transform.position.y,
                    cameraSettings.SizeFromMidPoint - zDistance),
                new Vector3(
                    mainCamera.transform.position.x,
                    mainCamera.transform.position.y,
                     -cameraSettings.SizeFromMidPoint - zDistance / 2)
            };

            for (int i = 0; i < listOfAddition.Count; i++)
            {
                var result = manager.ClampCameraPos(mainCamera, listOfAddition[i]);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 1000), Mathf.Round(expectedResults[i].y * 1000), Mathf.Round(expectedResults[i].z * 1000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 1000), Mathf.Round(result.y * 1000), Mathf.Round(result.z * 1000));
                Assert.AreEqual(expected, roundedResult);
            }

        }

        [Test]
        public void ShouldNotDoCameraMovement()
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            var result = manager.DoCameraMovement(0f, 0f, mousePos, deltaTime, mainCamera);
            Assert.AreEqual(mainCamera.transform.position, result);
        }

        [Test]
        public void ShouldDoCameraMovementWhenMouseOnBoundries()
        {
            var listOfMousePos = new List<Vector2>()
            {
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(1f, 0f),
                new Vector2(1f, 1f),
                new Vector2(0.5f, 0f),
                new Vector2(0f, 0.5f),
                new Vector2(0.5f, 1f),
                new Vector2(1f, 0.5f),
                new Vector2(2f, 0.5f),
                new Vector2(0.5f, 2f),
            };
            var expectedResults = new List<Vector3>()
            {
                new Vector3(-.7080f, 20f, -20.7080f),
                new Vector3(-.7080f, 20f, -19.2920f),
                new Vector3(.7080f, 20f, -20.7080f),
                new Vector3(.7080f, 20f, -19.2920f),
                new Vector3(0f, 20f, -20.7080f),
                new Vector3(-.7080f, 20f, -20),
                new Vector3(0f, 20f, -19.2920f),
                new Vector3(.7080f, 20f, -20),
                new Vector3(0, 20f, -20),
                new Vector3(0, 20f, -20),

            };

            for (int i = 0; i < listOfMousePos.Count; i++)
            {
                Vector3 mousePos = mainCamera.ViewportToScreenPoint(listOfMousePos[i]);
                var result = manager.DoCameraMovement(0f, 0f, mousePos, deltaTime, mainCamera);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 10000), Mathf.Round(expectedResults[i].y * 10000), Mathf.Round(expectedResults[i].z * 10000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
                Assert.AreEqual(expected, roundedResult);
            }
        }

        [Test]
        public void ShouldDoCameraMovementWhenAxisPressed()
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            var listOfMousePos = new List<Vector2>()
            {
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(1f, 0f),
                new Vector2(1f, 1f),
                new Vector2(0f, -1f),
                new Vector2(-1f, 0f),
                new Vector2(-1f, -1f),
                new Vector2(-1f, 1f),
                new Vector2(1f, -1f),
                new Vector2(.02f, 0f),
                new Vector2(0f, .02f),
            };
            var expectedResults = new List<Vector3>()
            {
                new Vector3(0f, 20f, -20f),
                new Vector3(0f, 20f, -19.2920f),
                new Vector3(.7080f, 20f, -20f),
                new Vector3(.7080f, 20f, -19.2920f),
                new Vector3(0f, 20f, -20.7080f),
                new Vector3(-.7080f, 20f, -20f),
                new Vector3(-.7080f, 20f, -20.7080f),
                new Vector3(-.7080f, 20f, -19.2920f),
                new Vector3(.7080f, 20f, -20.7080f),
                new Vector3(0f, 20f, -20f),
                new Vector3(0f, 20f, -20f),

            };

            for (int i = 0; i < listOfMousePos.Count; i++)
            {
                var result = manager.DoCameraMovement(listOfMousePos[i].x, listOfMousePos[i].y, mousePos, deltaTime, mainCamera);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 10000), Mathf.Round(expectedResults[i].y * 10000), Mathf.Round(expectedResults[i].z * 10000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
                Assert.AreEqual(expected, roundedResult);
            }
        }

        [Test]
        public void ShouldCenterCameraWhenDoCameraMovement()
        {
            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
            float zDistance = 11.547f; //mocked z distance
            manager.IsCentering = true;
            List<SelectableObject> currentSelection = new List<SelectableObject>();
            SelectableObject obj = CreateGameObject<SelectableObject>();
            obj.transform.position = new Vector3(0, 0, 0);
            currentSelection.Add(obj);

            selectionManager.CurrentSelection.Returns(currentSelection);
            selectionManager.GetSelectionMainPoint().Returns(obj.transform.position);

            var result = manager.DoCameraMovement(0f, 0f, mousePos, deltaTime, mainCamera);
            Vector3 expected = new Vector3(Mathf.Round(obj.transform.position.x * 10000), Mathf.Round(mainCamera.transform.position.y * 10000), Mathf.Round((obj.transform.position.z - zDistance) * 10000));
            Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
            Assert.AreEqual(expected, roundedResult);

        }

        [Test]
        public void ShouldReturnVectorZeroWhenNotPanning_DoCameraPanning()
        {

            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(1f, 1f));
            manager.IsPanning = false;

            var result = manager.DoCameraPanning(mousePos, deltaTime, mainCamera);

            Assert.AreEqual(Vector3.zero, result);

        }

        [Test]
        public void ShouldReturnVectorZeroWhenCentering_DoCameraPanning()
        {

            Vector3 mousePos = mainCamera.ViewportToScreenPoint(new Vector2(1f, 1f));
            manager.IsPanning = true;
            manager.IsCentering = true;

            var result = manager.DoCameraPanning(mousePos, deltaTime, mainCamera);

            Assert.AreEqual(Vector3.zero, result);

        }

        [Test]
        public void ShouldDoCameraPanning()
        {

            var listOfMousePos = new List<Vector2>()
            {
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(1f, 0f),
                new Vector2(1f, 1f),
                new Vector2(0f, -1f),
                new Vector2(-1f, 0f),
                new Vector2(-1f, -1f),
                new Vector2(-1f, 1f),
                new Vector2(1f, -1f),
            };
            var expectedResults = new List<Vector3>()
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, -8.6603f, -5f),
                new Vector3(-10f, 0f, 0f),
                new Vector3(-10f, -8.6603f, -5f),
                new Vector3(0f, 8.6603f, 5f),
                new Vector3(10f, 0f, 0f),
                new Vector3(10f,8.6603f, 5f),
                new Vector3(10f,-8.6603f, -5f),
                new Vector3(-10f,8.6603f, 5f),

            };

            manager.IsPanning = true;
            manager.IsCentering = false;
            cameraSettings.PanSpeed = 5f;

            for (int i = 0; i < listOfMousePos.Count; i++)
            {
                Vector2 item = (Vector2)listOfMousePos[i];
                var result = manager.DoCameraPanning(new Vector2(listOfMousePos[i].x, listOfMousePos[i].y), deltaTime, mainCamera);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 10000), Mathf.Round(expectedResults[i].y * 10000), Mathf.Round(expectedResults[i].z * 10000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
                Assert.AreEqual(expected, roundedResult);
            }

        }

        [Test]
        public void ShouldDoCameraZoom()
        {
            cameraSettings.MinZoom = 3f;
            cameraSettings.MaxZoom = 30f;
            cameraSettings.ZoomScale = 10f;
            var mouseScrollDeltaList = new List<float>()
            {
                0f,
                1f,
                -1f,
                .5f,
                -.5f,
                .2f,
                -.8f
            };
            var expectedResults = new List<Vector3>()
            {
                new Vector3(0f, 20f, -20f),
                new Vector3(0f, 19.134f, -19.5f),
                new Vector3(0f, 20.866f, -20.5f),
                new Vector3(0f, 19.567f, -19.75f),
                new Vector3(0f, 20.433f, -20.25f),
                new Vector3(0f, 19.8268f, -19.9f),
                new Vector3(0f, 20.6928f, -20.4f),
            };

            manager.IsPanning = true;
            manager.IsCentering = false;
            cameraSettings.PanSpeed = 5f;

            for (int i = 0; i < mouseScrollDeltaList.Count; i++)
            {
                float item = (float)mouseScrollDeltaList[i];
                var result = manager.DoCameraZoom(mouseScrollDeltaList[i], deltaTime, mainCamera);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 10000), Mathf.Round(expectedResults[i].y * 10000), Mathf.Round(expectedResults[i].z * 10000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
                Assert.AreEqual(expected, roundedResult);
            }

        }

        [Test]
        public void ShouldDoCameraZoomWithClampedReturn()
        {
            cameraSettings.MinZoom = 19.5f;
            cameraSettings.MaxZoom = 20.5f;
            cameraSettings.ZoomScale = 10f;
            var mouseScrollDeltaList = new List<float>()
            {
                1f,
                -1f,
            };
            var expectedResults = new List<Vector3>()
            {
                new Vector3(0f, cameraSettings.MinZoom, -19.634f),
                new Vector3(0f, cameraSettings.MaxZoom, -20.366f),
            };

            manager.IsPanning = true;
            manager.IsCentering = false;
            cameraSettings.PanSpeed = 5f;

            for (int i = 0; i < mouseScrollDeltaList.Count; i++)
            {
                float item = (float)mouseScrollDeltaList[i];
                var result = manager.DoCameraZoom(mouseScrollDeltaList[i], deltaTime, mainCamera);
                Vector3 expected = new Vector3(Mathf.Round(expectedResults[i].x * 10000), Mathf.Round(expectedResults[i].y * 10000), Mathf.Round(expectedResults[i].z * 10000));
                Vector3 roundedResult = new Vector3(Mathf.Round(result.x * 10000), Mathf.Round(result.y * 10000), Mathf.Round(result.z * 10000));
                Assert.AreEqual(expected, roundedResult);
            }

        }

        // TODO panning clamp boundries
        // TODO panning dont move on z after minZoom or maxZoom


        #region methods

        public static T CreateGameObject<T>() where T : MonoBehaviour
        {
            var go = new GameObject();
            var so = go.AddComponent<T>();
            return so;
        }

        #endregion

    }
}
