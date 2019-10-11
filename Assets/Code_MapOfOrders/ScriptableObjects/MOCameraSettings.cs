using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 25)] public float zoomValue;
        [Range(0, 180)] public float cameraFov;
        [Range(50, 200)] public float zoomSpeed;
        [Range(0.1f, 100f)] public float mouseMapScrollSpeedMultiplier;
        [Range(0.1f, 2000f)] public float mouseMapDragSpeedMultiplier;

        public Vector3 cameraMapSpawnPosition;
        public Vector3 cameraLookAtAngle;
    }
}