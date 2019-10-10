using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 25)] public float maxZoomIn;
        [Range(0, 25)] public float maxZoomOut;
        [Range(0, 180)] public float cameraFov;
        [Range(50, 200)] public float zoomSpeed;
        [Range(0.1f, 100f)] public float mouseMapScrollSpeedScaler;
        [Range(0.1f, 50f)] public float mouseMapDragSpeedScaler;

        public Vector3 cameraMapSpawnPosition;
        public Vector3 cameraLookAtAngle;
    }
}