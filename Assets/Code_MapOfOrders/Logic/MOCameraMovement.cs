using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public abstract class MOCameraMovement : MonoBehaviour {
        [SerializeField] protected TweenSetup tweenSetup;

        protected MOCameraSettings cameraSettings;
        protected Camera thisCamera;
        protected Transform thisTransform;
        protected Vector3 tweenTargetPos;
        protected Tweener tweener;

        bool initialised;

        protected abstract void UpdatePosition();

        void Start() {
            Initialise();
        }

        protected virtual void Initialise() {
            if (initialised)
                return;

            initialised = true;
        }

        protected void PlayTween() {
            tweener.ChangeEndValue(tweenTargetPos, true)
                 .Restart();
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
            tweener = thisTransform
                .DOLocalMove(thisTransform.localPosition, tweenSetup.tweenSettings.tweenTime)
                .SetAutoKill(false)
                .SetEase(tweenSetup.tweenSettings.easeType);
        }
    }
}