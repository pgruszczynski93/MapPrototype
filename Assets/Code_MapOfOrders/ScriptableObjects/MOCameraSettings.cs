using UnityEngine;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 25)] public float maxZoomValue;
        [Range(0, 180)] public float cameraFov;
        [Range(0, 10)] public float zoomDistanceStep;
        [Range(0f, 1f)] public float smoothZoomTime;
        [Range(0f, 1f)] public float cameraMoveSmoothing;
        [Range(0.1f, 100f)] public float mouseMapScrollSpeedMultiplier;
        [Range(0.1f, 500f)] public float mouseMapDragSpeedMultiplier;
        [Range(0.1f, 10f)] public float mouseMapDragSensitivity;

        public Vector3 cameraMapSpawnPosition;
        public Vector3 cameraLookAtAngle;
    }
}