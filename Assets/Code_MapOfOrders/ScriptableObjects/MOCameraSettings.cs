using DG.Tweening;
using UnityEngine;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 25)] public float maxZoomValue;
        [Range(0, 180)] public float cameraFov;

        public Vector3 cameraMapSpawnPosition;
        public Vector3 cameraLookAtAngle;
    }
}