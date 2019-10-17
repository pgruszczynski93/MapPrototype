using System;
using System.Collections;
using System.Numerics;
using Code_MapOfOrders.Logic;
using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace DefaultNamespace {
    [RequireComponent(typeof(Camera), typeof(MOCameraMovementArea))]
    public class MOCameraMovement : MonoBehaviour {
        [SerializeField] MOCameraSettings cameraSettings;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] Camera thisCamera;
        [SerializeField] Transform thisTransform;
        [SerializeField] MOCameraMovementArea cameraMovementArea;
        [SerializeField] MOHouseSelector houseSelector;
        [SerializeField] MOBorders movementBorders;

        bool isZooming;
        bool isSelected;
        bool initialised;

        float dt;
        float minZoom;
        float maxZoom;
        float currentZoom;

        MOMouseInputData mouseInputData;

        void Start() {
            Initialise();
        }

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            movementBorders = cameraMovementArea.MapBorders;
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
            MOEvents.OnLateUpdate += HandleMouseMovementActions;
        }

        void RemoveEvents() {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseMovementActions;
        }

        void ClampCameraMovement() {
            var clampedLocalPos = thisTransform.localPosition;
            clampedLocalPos.x = Mathf.Clamp(clampedLocalPos.x, movementBorders.left, movementBorders.right);
            clampedLocalPos.y = Mathf.Clamp(clampedLocalPos.y, minZoom, maxZoom);
            clampedLocalPos.z = Mathf.Clamp(clampedLocalPos.z, movementBorders.bottom, movementBorders.top);
            thisTransform.localPosition = clampedLocalPos;
        }

        void LoadAndApplySettings() {
            cameraSettings = cameraSetup.cameraSettings;
            thisCamera.fieldOfView = cameraSettings.cameraFov;
            thisTransform.localPosition = cameraSettings.cameraMapSpawnPosition;
            thisTransform.localRotation = Quaternion.Euler(cameraSettings.cameraLookAtAngle);
            var startHeight = thisTransform.localPosition.y;
            currentZoom = startHeight;
            minZoom = startHeight - cameraSettings.maxZoomValue;
            maxZoom = startHeight + cameraSettings.maxZoomValue;
        }

        void HandleMouseInputCollectedReceived(MOMouseInputData inputData) {
            mouseInputData = inputData;
        }

        void HandleMouseMovementActions() {
            dt = Time.deltaTime;

            switch (mouseInputData.mouseAction) {
                case MouseAction.Undefined:
                    //ResetSelectingPossibility();
                    break;
                case MouseAction.MapSelection:
                    //TryToSelect();
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

            if (isZooming == false) {
//                TryToZoomMap();
            }

            ClampCameraMovement();
        }

        void ResetSelectingPossibility() {
            isSelected = false;

            houseSelector.TryToHighlightObject(thisCamera.ScreenPointToRay(mouseInputData.pointerPosition));
        }

        void TryToSelect() {
            if (isSelected)
                return;
            Debug.Log("[MOCameraMovement] Select");
            isSelected = true;
            houseSelector.Show(thisCamera.ScreenPointToRay(mouseInputData.pointerActionPosition));
        }

        void TryToInvokeDragMapMovement() {
            if (mouseInputData.mouseAction != MouseAction.MapDragMovement)
                return;

            Debug.Log("[MOCameraMovement] Drag");
            var pointerPosition = mouseInputData.pointerActionPosition;
            var dragPos = new Vector3(pointerPosition.x, 0, pointerPosition.y);
            UpdateCameraPosition(dragPos, cameraSetup.cameraSettings.dragTweenProperties);
        }


//        void TryToZoomMap()
//        {
//            var scrollValue = mouseInputData.scrollValue;
//            if (scrollValue == 0 || isZooming)
//                return;
//
//            Debug.Log("[MOCameraMovement] Zoom");
//            var zoomDelta = cameraSettings.zoomDistanceStep * Mathf.Sign(scrollValue);
//            var currentPos = thisTransform.localPosition;
//            var nextZoom = currentZoom - zoomDelta;
//            currentZoom = nextZoom;
//            if (nextZoom >= minZoom && nextZoom <= maxZoom)
//            {
//                var scrollVec = currentPos + zoomDelta * thisTransform.forward;
//                StartCoroutine(SmoothZoom(currentPos, scrollVec));
//            }
//            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
//        }
//
////todo: change it to smooth util (hf) and ease in out animation
//        IEnumerator SmoothZoom(Vector3 from, Vector3 to, Action onEnd = null)
//        {
//            var t = 0f;
//            isZooming = true;
//            while (t < cameraSettings.smoothZoomTime)
//            {
//                t += dt;
//                transform.position = Vector3.Lerp(from, to, t / cameraSettings.smoothZoomTime);
//                yield return null;
//            }
//
//            isZooming = false;
//        }
        /// /////////////////////////////////
        void TryToInvokeScrollMapMovement() {
            switch (mouseInputData.mouseAction) {
                case MouseAction.MapScrollMovement: {
                    Debug.Log("[MOCameraMovement] Scroll map");
                    var mapScrollDirection = PointerEdgePositionToScrollDirection();
                    PointerEdgePositionToScrollDirection();
                    UpdateCameraPosition(mapScrollDirection, cameraSetup.cameraSettings.scrollTweenProperties);
                    break;
                }
                case MouseAction.Undefined:
                    thisTransform.DOPause();
                    break;
            }
        }

        Vector3 PointerEdgePositionToScrollDirection() {
            var pointerPosition = mouseInputData.pointerActionPosition;
            var scrollDirectionX = RescaleViewportMinMaxCoordinate(pointerPosition.x, -1, 1f);
            var scrollDirectionZ = RescaleViewportMinMaxCoordinate(pointerPosition.y, -1, 1f);
            return new Vector3(scrollDirectionX, 0, scrollDirectionZ).normalized;
        }

        float RescaleViewportMinMaxCoordinate(float oldVal, float newMin, float newMax) {
            return (oldVal) * (newMax - newMin) + newMin;
        }

        void UpdateCameraPosition(Vector3 deltaPosition, TweenProperty tweenProperty) {
            var currentPos = thisTransform.localPosition;
            var desiredPosChange = deltaPosition * tweenProperty.positionDeltaMultiplier;
            var newPos = currentPos + desiredPosChange;
            thisTransform.DOLocalMove(newPos, tweenProperty.tweenTime).SetEase(tweenProperty.easeType);
        }
    }
}