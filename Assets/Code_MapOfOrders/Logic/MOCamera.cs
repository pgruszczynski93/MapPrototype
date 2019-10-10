using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace {
    [RequireComponent(typeof(Camera))]
    public class MOCamera : MonoBehaviour {
        [SerializeField] MOCameraSettings cameraSettings;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] Camera thisCamera;
        [SerializeField] Transform thisTransform;

        bool initialised;

        float lastPointerMovementMagnitude;
        float maxZoomIn;
        float maxZoomOut;
        Vector3 lastPointerMovementDelta;

        void Start() {
            Initialise();
        }

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            LoadAndApplySettings();
        }

        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            
            MOEvents.OnMouseInputCollected += HandleMouseInputCollectedReceived;
        }

        void RemoveEvents() {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
        }

        void LoadAndApplySettings() {
            cameraSettings = cameraSetup.cameraSettings;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
            var localPosZ = thisTransform.localPosition.z;
            maxZoomIn = localPosZ + cameraSettings.maxZoomIn;
            maxZoomOut = localPosZ + cameraSettings.maxZoomOut;
        }

        void HandleMouseInputCollectedReceived(MOMouseInputData inputData) {
            HandleMouseAction(inputData);
        }

        void HandleMouseAction(MOMouseInputData inputData) {
            switch (inputData.mouseAction) {
                case MouseAction.Stopped:
                    break;
                case MouseAction.MapSelection:
                    break;
                case MouseAction.MapDragMovement:
                    TryToInvokeDragMapMovement(inputData.pointerPositionDelta);
                    break;
                case MouseAction.MapScrollMovement:
                    TryToInvokeScrollMapMovement(inputData.pointerPositionDelta);
                    break;
                default:
                    break;
            }
            
            TryToZoomMap(inputData.scrollValue);
        }

        void TryToInvokeDragMapMovement(Vector3 pointerPosDelta) {
            if (!(pointerPosDelta.sqrMagnitude > 0))
                return;
            
            
        }
        

        void TryToZoomMap(float scrollData) {
            thisTransform.Translate((scrollData * cameraSettings.zoomSpeed * Vector3.forward * Time.deltaTime));
//            var localPos = thisTransform.localPosition;
//            thisTransform.localPosition = new Vector3(localPos.x, localPos.y, Mathf.Clamp(localPos.z, maxZoomIn, maxZoomOut));
        }

        void TryToInvokeScrollMapMovement(Vector3 pointerPosDelta) {
            if (pointerPosDelta.sqrMagnitude > 0)
                SetLastPointerOutOfViewportTranslation(pointerPosDelta);
            
            SetCameraPositionWhenScrolling(pointerPosDelta);
        }

        void SetLastPointerOutOfViewportTranslation(Vector3 pointerPosDelta) {
            lastPointerMovementDelta = new Vector3(pointerPosDelta.x, 0f, pointerPosDelta.y).normalized;
        }

        void SetCameraPositionWhenScrolling(Vector3 pointerPosDelta) {
            var currentPosition = thisTransform.localPosition;
            var newPosition = currentPosition + lastPointerMovementDelta;
            var smoothedPosition =
                Vector3.Slerp(currentPosition, newPosition, Time.deltaTime * cameraSettings.mouseMapScrollSpeedScaler);
            thisTransform.localPosition = new Vector3(smoothedPosition.x, currentPosition.y, smoothedPosition.z);
        }
    }
}