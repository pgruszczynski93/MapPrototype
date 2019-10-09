using UnityEngine;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 180)] public float cameraFov;
        [Range(0, 25)] public float maxZoomIn;
        [Range(0, 25)] public float maxZoomOut;
        [Range(0.1f, 100f)] public float mouseSpeedScaler;
    }
}