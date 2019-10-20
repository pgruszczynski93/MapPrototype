using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public abstract class MOCameraMovement : MonoBehaviour
    {
        [SerializeField] protected MOCameraSettings cameraSettings;
        [SerializeField] protected MOCameraSetup cameraSetup;
        [SerializeField] protected TweenSetup tweenSetup;

        [SerializeField] protected Camera thisCamera;
        [SerializeField] protected Transform thisTransform;
        [SerializeField] protected MOHouseSelector houseSelector;

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
            LoadAndApplySettings();
        }

        void LoadAndApplySettings()
        {
            cameraSettings = cameraSetup.cameraSettings;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
        }
        
    }
}