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

//        void Update()
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                testPos = Input.mousePosition;
//                return;
//            }
// 
//            if (!Input.GetMouseButton(0) || testPos == Input.mousePosition)
//                return;
// 
//            Vector3 pos = Camera.main.ScreenToViewportPoint( testPos-Input.mousePosition);
//            Vector3 move = new Vector3(pos.x * 50, 0, pos.y * 50);
//            
//            Debug.Log("POS UPDA " + pos);
// 
//            transform.Translate(move, Space.World);
//            testPos = Input.mousePosition;
//        }
        
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
            var scaledPositionChangeSpeed = pointerPosition.sqrMagnitude;
            UpdateCameraPosition(dragPos, cameraSettings.mouseMapDragSpeedMultiplier,
                scaledPositionChangeSpeed);
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

        // tweak it:::: 
        void UpdateCameraPosition(Vector3 deltaPosition, float updateSpeed, float positionChangeSpeed = 1f) {
//            var desiredPosition = dt * deltaPosition * updateSpeed * positionChangeSpeed;
//            var desiredPosition = deltaPosition;
            var newPosition = new Vector3(10*deltaPosition.x , 0f, 10*deltaPosition.z  );
            thisTransform.Translate(newPosition, Space.World);
//            var smoothedPosition =
//                Vector3.Slerp(currentPosition, newPosition, dt * updateSpeed);
//            thisTransform.localPosition = new Vector3(smoothedPosition.x, currentPosition.y, smoothedPosition.z);
        }
    }
}