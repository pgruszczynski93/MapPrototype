using System;
using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public abstract class MOCameraMovement : MonoBehaviour {
        [SerializeField] protected TweenSetup tweenSetup;

        protected MOCameraSettings cameraSettings;
        protected Camera thisCamera;
        protected Transform thisTransform;
        protected Tweener tweener;

        bool initialised;

        protected abstract void UpdatePosition();

        protected virtual void Initialise() {
            if (initialised)
                return;

            initialised = true;
            tweener
                .SetAutoKill(false)
                .SetEase(tweenSetup.tweenSettings.easeType);
        }

        protected void PlayTween(Vector3 tweenTargetPos, Action onTweenComplete = null) {
            tweener = thisTransform
                .DOLocalMove(tweenTargetPos, tweenSetup.tweenSettings.tweenTime)
                .OnComplete(() => {
                    onTweenComplete?.Invoke(); 
                });
        }

        public void PauseTween() {
            tweener.Pause();
        }

        public void SetupMovement(MOCameraSetup setup, Camera cam) {
            cameraSettings = setup.cameraSettings;
            thisCamera = cam;
            thisTransform = cam.transform;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
            Initialise();
        }
    }
}