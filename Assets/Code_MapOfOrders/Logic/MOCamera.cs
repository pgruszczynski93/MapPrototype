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
        public Vector3 testOff;

        bool initialised;

        float dt;
        float lastPointerMovementMagnitude;
        float currentZoom;
        float minZoom;
        float maxZoom;


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
                    TryToInvokeScrollMapMovement(mouseInputData.pointerPosition);
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
            UpdateCameraPosition(dragPos, cameraSettings.mouseMapDragSpeedMultiplier,
                cameraSettings.mouseMapDragSensitivity);
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

            UpdateCameraPosition(lastPointerMovementDelta, cameraSettings.mouseMapScrollSpeedMultiplier,
                cameraSettings.mouseMapScrollSpeedSensitivity);
        }

        void SetLastPointerOutOfViewportTranslation(Vector3 pointerPosDelta) {
            lastPointerMovementDelta = new Vector3(pointerPosDelta.x, 0f, pointerPosDelta.y).normalized;
        }

        void UpdateCameraPosition(Vector3 deltaPosition, float multiplierSpeed, float sensitivity) {
            var currentPos = thisTransform.localPosition;
            var positionChangeMultiplier = multiplierSpeed * dt * sensitivity;
            var desiredPosChange = deltaPosition * positionChangeMultiplier;
            var newPos = currentPos + desiredPosChange;
            var smoothedPos = Vector3.Slerp(currentPos, newPos, 0.5f);
            var constantHeightSmoothedPos = new Vector3(smoothedPos.x, currentPos.y, smoothedPos.z);
            thisTransform.localPosition = constantHeightSmoothedPos;
        }
    }
}