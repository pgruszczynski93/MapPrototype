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

        float dt;
        float lastPointerMovementMagnitude;
        float currentZoom;
        float minZoom;
        float maxZoom;

        Vector3 testPos;
        
        MOMouseInputData mouseInputData;
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
            MOEvents.OnLateUpdate += HandleMouseActions;
        }

        void RemoveEvents() {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseActions;
        }

        void LoadAndApplySettings() {
            cameraSettings = cameraSetup.cameraSettings;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
            var localPosZ = thisTransform.localPosition.z;
            minZoom = localPosZ - cameraSettings.zoomValue;
            maxZoom = localPosZ + cameraSettings.zoomValue;
        }

        void HandleMouseInputCollectedReceived(MOMouseInputData inputData) {
            mouseInputData = inputData;
        }

        void HandleMouseActions() {
            dt = Time.deltaTime;

            switch (mouseInputData.mouseAction) {
                case MouseAction.Stopped:
                    break;
                case MouseAction.MapSelection:
                    break;
                case MouseAction.MapDragMovement:
                    TryToInvokeDragMapMovement(mouseInputData.pointerPosition);
                    break;
                case MouseAction.MapScrollMovement:
//                    TryToInvokeScrollMapMovement(mouseInputData.pointerPosition);
                    break;
                default:
                    break;
            }

            TryToZoomMap(mouseInputData.scrollValue);
        }

        void TryToInvokeDragMapMovement(Vector3 pointerPosition) {
            if (!(pointerPosition.sqrMagnitude > 0))
                return;

            var dragPos = new Vector3(pointerPosition.x, 0, pointerPosition.y);
            UpdateCameraPosition(dragPos, cameraSettings.mouseMapDragSpeedMultiplier);
        }


        void TryToZoomMap(float scrollData) {
//            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
//
//            Vector3 move = pos.y * scrollData * transform.forward; 
//            transform.Translate(move, Space.World);

        }

        void TryToInvokeScrollMapMovement(Vector3 pointerPosDelta) {
            if (pointerPosDelta.sqrMagnitude > 0)
                SetLastPointerOutOfViewportTranslation(pointerPosDelta);

            UpdateCameraPosition(lastPointerMovementDelta, cameraSettings.mouseMapScrollSpeedMultiplier);
        }

        void SetLastPointerOutOfViewportTranslation(Vector3 pointerPosDelta) {
            lastPointerMovementDelta = new Vector3(pointerPosDelta.x, 0f, pointerPosDelta.y).normalized;
        }

        void UpdateCameraPosition(Vector3 deltaPosition, float updateSpeed) {
            
            var desiredPosition = deltaPosition * updateSpeed * dt;
            thisTransform.Translate(desiredPosition, Space.World);
//            var smoothedPosition =
//                Vector3.Slerp(currentPosition, newPosition, dt * updateSpeed);
//            thisTransform.localPosition = new Vector3(smoothedPosition.x, currentPosition.y, smoothedPosition.z);
        }
    }
}