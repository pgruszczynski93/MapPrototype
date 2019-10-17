using DG.Tweening;
using UnityEngine;

namespace HGTV.MapsOfOrders {
    [System.Serializable]
    public struct MOCameraSettings {
        [Range(0, 25)] public float maxZoomValue;
        [Range(0, 180)] public float cameraFov;
        [Range(0, 10)] public float zoomDistanceStep;

        public TweenProperty dragTweenProperties;
        public TweenProperty scrollTweenProperties;
        public TweenProperty zoomTweenProperties;

        public Vector3 cameraMapSpawnPosition;
        public Vector3 cameraLookAtAngle;
    }

    [System.Serializable]
    public struct TweenProperty {
        public Ease easeType;
        [Range(0f, 10f)] public float tweenTime;

        [Tooltip("Set this for scroll & drag properties. For zooming is ignored."),
         Range(0f, 500f)]
        public float positionDeltaMultiplier;
    }
}