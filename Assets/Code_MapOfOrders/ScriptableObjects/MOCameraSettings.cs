using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 25)] public float zoomValue;
        [Range(0, 180)] public float cameraFov;
        [Range(50, 200)] public float zoomSpeed;
        [Range(0.1f, 100f)] public float mouseMapScrollSpeedMultiplier;
        [Range(1f, 10)] public float mouseMapScrollSpeedSensitivity;
        [Range(0.1f, 500f)] public float mouseMapDragSpeedMultiplier;
        [Range(0.1f, 10f)] public float mouseMapDragSensitivity;

        public Vector3 cameraMapSpawnPosition;
        public Vector3 cameraLookAtAngle;
    }
}