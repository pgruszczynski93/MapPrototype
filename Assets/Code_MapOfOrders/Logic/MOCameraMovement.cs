using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public abstract class MOCameraMovement : MonoBehaviour
    {
        [SerializeField] protected TweenSetup tweenSetup;

        protected MOCameraSettings cameraSettings;
        protected Camera thisCamera;
        protected Transform thisTransform;

        bool initialised;

        protected abstract void UpdatePosition();

        void Start()
        {
            Initialise();
        }

        protected virtual void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
        }

        public void SetupMovement(MOCameraSetup setup, Camera cam)
        {
            cameraSettings = setup.cameraSettings;
            thisCamera = cam;
            thisTransform = cam.transform;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
        }
    }
}