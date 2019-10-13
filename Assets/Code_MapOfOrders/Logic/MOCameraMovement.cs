using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Camera), typeof(MOCameraMovementArea))]
    public class MOCameraMovement : MonoBehaviour
    {
        [SerializeField] MOCameraSettings cameraSettings;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] Camera thisCamera;
        [SerializeField] Transform thisTransform;
        [SerializeField] MOCameraMovementArea cameraMovementArea;
        [SerializeField] private MOBorders movementBorders;

        bool initialised;

        float dt;
        float currentZoom;
        float minZoom;
        float maxZoom;

        MOMouseInputData mouseInputData;

        void Start()
        {
            Initialise();
        }

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            movementBorders = cameraMovementArea.MapBorders;
            LoadAndApplySettings();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            MOEvents.OnMouseInputCollected += HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate += HandleMouseActions;
            MOEvents.OnLateUpdate += ClampCameraMovement;
        }

        void RemoveEvents()
        {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseActions;
            MOEvents.OnLateUpdate -= ClampCameraMovement;
        }

        void ClampCameraMovement()
        {
            var clampedLocalPos = thisTransform.localPosition;
            clampedLocalPos.x = Mathf.Clamp(clampedLocalPos.x, movementBorders.left, movementBorders.right);
            clampedLocalPos.y = Mathf.Clamp(clampedLocalPos.y, minZoom, maxZoom);
            clampedLocalPos.z = Mathf.Clamp(clampedLocalPos.z, movementBorders.bottom, movementBorders.top);
            thisTransform.localPosition = clampedLocalPos;
        }

        void LoadAndApplySettings()
        {
            cameraSettings = cameraSetup.cameraSettings;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
            var startHeight = thisTransform.localPosition.y;
            currentZoom = startHeight;
            minZoom = startHeight - cameraSettings.zoomValue;
            maxZoom = startHeight + cameraSettings.zoomValue;
        }

        void HandleMouseInputCollectedReceived(MOMouseInputData inputData)
        {
            mouseInputData = inputData;
        }

        void HandleMouseActions()
        {
            dt = Time.deltaTime;

            switch (mouseInputData.mouseAction)
            {
                case MouseAction.Stopped:
                    break;
                case MouseAction.MapSelection:
                    break;
                case MouseAction.MapDragMovement:
                    TryToInvokeDragMapMovement();
                    break;
                case MouseAction.MapScrollMovement:
                    TryToInvokeScrollMapMovement();
                    break;
                default:
                    break;
            }

            TryToZoomMap(mouseInputData.scrollValue);
        }

        void TryToInvokeDragMapMovement()
        {
            if (mouseInputData.mouseAction == MouseAction.Stopped)
                return;

            var pointerPosition = mouseInputData.pointerPosition;
            var dragPos = new Vector3(pointerPosition.x, 0, pointerPosition.y);
            UpdateCameraPosition(dragPos, cameraSettings.mouseMapDragSpeedMultiplier,
                cameraSettings.mouseMapDragSensitivity);
        }


        void TryToZoomMap(float scrollData)
        {
            currentZoom += scrollData ;
            if (currentZoom >= minZoom && currentZoom <= maxZoom)
            {
                var scrollVector = /*cameraSettings.mouseZoomSensitivity * dt */ scrollData * Vector3.forward;
                transform.Translate(scrollVector, Space.Self);
            }

            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        void TryToInvokeScrollMapMovement()
        {
            if (mouseInputData.mouseAction != MouseAction.MapScrollMovement)
                return;

            var mapScrollDirection = PointerEdgePositionToScrollDirection();
            PointerEdgePositionToScrollDirection();
            UpdateCameraPosition(mapScrollDirection, cameraSettings.mouseMapScrollSpeedMultiplier,
                cameraSettings.mouseMapDragSensitivity);
        }

        Vector3 PointerEdgePositionToScrollDirection()
        {
            var pointerPosition = mouseInputData.pointerPosition;
            var scrollDirectionX = RescaleViewportMinMaxCoordinate(pointerPosition.x, -1, 1f);
            var scrollDirectionZ = RescaleViewportMinMaxCoordinate(pointerPosition.y, -1, 1f);
            return new Vector3(scrollDirectionX, 0, scrollDirectionZ).normalized;
        }

        float RescaleViewportMinMaxCoordinate(float oldVal, float newMin, float newMax)
        {
            return (oldVal) * (newMax - newMin) + newMin;
        }

        void UpdateCameraPosition(Vector3 deltaPosition, float multiplierSpeed, float sensitivity)
        {
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