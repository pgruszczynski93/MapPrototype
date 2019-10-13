using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Camera))]
    public class MOCamera : MonoBehaviour
    {
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
        }

        void RemoveEvents()
        {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseActions;
        }

        void LoadAndApplySettings()
        {
            cameraSettings = cameraSetup.cameraSettings;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
            var localPosZ = thisTransform.localPosition.z;
            minZoom = localPosZ - cameraSettings.zoomValue;
            maxZoom = localPosZ + cameraSettings.zoomValue;
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
//            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
//
//            Vector3 move = pos.y * scrollData * transform.forward; 
//            transform.Translate(move, Space.World);
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