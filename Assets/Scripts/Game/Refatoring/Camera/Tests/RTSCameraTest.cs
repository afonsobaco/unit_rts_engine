using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RTSEngine.Refactoring;
using NSubstitute;
using System;

namespace Tests
{

    [TestFixture]
    public class RTSCameraTest
    {

        private RTSCamera camera;
        const float CAMERA_SPEED = 1f;
        const float DELTA_TIME = 1f;
        const float CAMERA_HEIGHT = 1f;

        [SetUp]
        public void SetUp()
        {
            camera = Substitute.ForPartsOf<RTSCamera>();
        }

        [Test]
        public void RTSCameraTestSimplePasses()
        {
            Assert.NotNull(camera);
        }

        [TestCaseSource(nameof(CameraMovementScenarios))]
        public void ShouldGetCameraMovement(float horizontal, float vertical, Vector3 expected)
        {
            camera.WhenForAnyArgs(x => x.GetCameraSpeedByHeight(default)).DoNotCallBase();
            camera.GetCameraSpeedByHeight(default).ReturnsForAnyArgs(CAMERA_SPEED);
            Vector3 result = camera.GetCameraMovement(horizontal, vertical, CAMERA_HEIGHT, DELTA_TIME);
            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(CameraSpeedScenarios))]
        public void ShouldGetCameraSpeed(float height, float speed)
        {
            camera.MoveSpeed = speed;
            var result = camera.GetCameraSpeedByHeight(height);
            float expected = height * speed + RTSCamera.MAGIC_NUMBER;
            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(CameraZoomScenarios))]
        public void ShouldGetCameraZoom(Vector3 forward, float zoom)
        {
            camera.ZoomSpeed = CAMERA_SPEED;
            Vector3 result = camera.GetCameraZoom(zoom, forward, DELTA_TIME);
            Vector3 expected = CAMERA_SPEED * DELTA_TIME * zoom * forward;
            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(CameraPanScenarios))]
        public void ShouldGetCameraPan(Vector2 mouseAxis, float height)
        {
            camera.PanSpeed = CAMERA_SPEED;
            Vector3 result = camera.GetCameraPan(mouseAxis, height, DELTA_TIME);
            Vector3 expected = new Vector3(-mouseAxis.x, 0f, -mouseAxis.y) * height * CAMERA_SPEED * DELTA_TIME;
            Assert.AreEqual(expected, result);
        }

        private static IEnumerable<TestCaseData> CameraPanScenarios
        {
            get
            {
                yield return new TestCaseData(Vector2.one, 5f);
                yield return new TestCaseData(Vector2.one * -1, 5f);
                yield return new TestCaseData(Vector2.up, 20f);
                yield return new TestCaseData(Vector2.down, 10f);
                yield return new TestCaseData(Vector2.left, 20f);
                yield return new TestCaseData(Vector2.right, 5f);
            }
        }

        private static IEnumerable<TestCaseData> CameraZoomScenarios
        {
            get
            {
                yield return new TestCaseData(Vector3.one, 1f);
                yield return new TestCaseData(Vector3.up, -1f);
                yield return new TestCaseData(Vector3.down, 1f);
                yield return new TestCaseData(Vector3.left, -1f);
                yield return new TestCaseData(Vector3.right, 1f);
            }
        }

        private static IEnumerable<TestCaseData> CameraSpeedScenarios
        {
            get
            {
                yield return new TestCaseData(1f, 5f);
                yield return new TestCaseData(10f, 6f);
                yield return new TestCaseData(20f, 7f);
            }
        }

        private static IEnumerable<TestCaseData> CameraMovementScenarios
        {
            get
            {
                yield return new TestCaseData(0f, 0f, new Vector3(0f, 0f, 0f)).SetName("Stay at same position");
                yield return new TestCaseData(0f, 1f, new Vector3(0f, 0f, 1f)).SetName("Move North");
                yield return new TestCaseData(0f, -1f, new Vector3(0f, 0f, -1f)).SetName("Move South");
                yield return new TestCaseData(1f, 0f, new Vector3(1f, 0f, 0f)).SetName("Move East");
                yield return new TestCaseData(1f, 1f, new Vector3(1f, 0f, 1f)).SetName("Move NorthEast");
                yield return new TestCaseData(1f, -1f, new Vector3(1f, 0f, -1f)).SetName("Move SouthEast");
                yield return new TestCaseData(-1f, 0f, new Vector3(-1f, 0f, 0f)).SetName("Move West");
                yield return new TestCaseData(-1f, 1f, new Vector3(-1f, 0f, 1f)).SetName("Move NorthWest");
                yield return new TestCaseData(-1f, -1f, new Vector3(-1f, 0f, -1f)).SetName("Move SouthWest");
            }
        }
    }
}
