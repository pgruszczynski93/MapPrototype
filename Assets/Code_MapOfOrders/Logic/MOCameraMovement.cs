using System;
using System.Collections;
using System.Numerics;
using Code_MapOfOrders.Logic;
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
        [SerializeField] MOBorders movementBorders;

        bool isZooming;
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
            MOEvents.OnLateUpdate += HandleMouseActions;
//            MOEvents.OnLateUpdate += ClampCameraMovement;
        }

        void RemoveEvents() {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseActions;
//            MOEvents.OnLateUpdate -= ClampCameraMovement;
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

        void HandleMouseActions() {
            dt = Time.deltaTime;

            switch (mouseInputData.mouseAction) {
                case MouseAction.Stopped:
                    break;
                case MouseAction.MapSelection:
                    break;
                case MouseAction.MapDragMovement:
                    TryToInvokeDragMapMovement();
                    break;
                case MouseAction.MapScrollMovement:
//                    TryToInvokeScrollMapMovement();
                    break;
                default:
                    break;
            }

            if (isZooming == false) {
                TryToZoomMap();
            }

            ClampCameraMovement();
        }

        void TryToInvokeDragMapMovement() {
            if (mouseInputData.mouseAction == MouseAction.Stopped)
                return;

            var pointerPosition = mouseInputData.pointerPosition;
            var dragPos = new Vector3(pointerPosition.x, 0, pointerPosition.y);
            UpdateCameraPosition(dragPos, cameraSettings.mouseMapDragSpeedMultiplier,
                cameraSettings.mouseMapDragSensitivity);
        }


        void TryToZoomMap() {
            var scrollValue = mouseInputData.scrollValue;
            if (scrollValue == 0 || isZooming)
                return;

            var scrollSgn = Mathf.Sign(scrollValue);
            var zoomDelta = cameraSettings.zoomDistanceStep * scrollSgn * dt;
            var currentPos = thisTransform.localPosition;
            var nextZoom = currentZoom - zoomDelta;
            var cameraHeight = currentPos.y;
            Debug.Log("x " + nextZoom);

//            if (nextZoom < minZoom && cameraHeight > minZoom) {
//                var scrollVec = currentPos + (minZoom - nextZoom) * thisTransform.forward;
//                currentZoom = minZoom;
//                Debug.Log("MIN");
//                StartCoroutine(SmoothZoom(currentPos, scrollVec, () => {
//                    var lp = thisTransform.localPosition;
//                    lp.y = minZoom;
//                    thisTransform.localPosition = lp;
//                }));
//            }
//            else if (nextZoom > maxZoom && cameraHeight < maxZoom) {
//                var scrollVec = currentPos + (maxZoom - nextZoom) * thisTransform.forward;
//                currentZoom = maxZoom;
//                Debug.Log("MAX");
//                StartCoroutine(SmoothZoom(currentPos, scrollVec, () => {
//                    var lp = thisTransform.localPosition;
//                    lp.y = maxZoom;
//                    thisTransform.localPosition = lp;
//                }));
//            } 
//            else if(nextZoom > minZoom && nextZoom < maxZoom) {
            currentZoom = nextZoom;
            if(nextZoom > minZoom && nextZoom < maxZoom) {
                Debug.Log("ZOOM");
                var scrollVec = currentPos + zoomDelta * thisTransform.forward;
                thisTransform.Translate( zoomDelta * Vector3.forward, Space.Self );
//                StartCoroutine(SmoothZoom(currentPos, scrollVec));
            }
            else {
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            }
        }

//todo: change it to smooth util (hf) and ease in out animation
        IEnumerator SmoothZoom(Vector3 from, Vector3 to, Action onEnd = null) {
            var t = 0f;
            isZooming = true;
            while (t < cameraSettings.smoothZoomTime) {
                t += dt;
                transform.position = Vector3.Lerp(from, to, t / cameraSettings.smoothZoomTime);
                yield return null;
            }

            isZooming = false;
            onEnd?.Invoke();
        }

        void TryToInvokeScrollMapMovement() {
            if (mouseInputData.mouseAction != MouseAction.MapScrollMovement)
                return;

            var mapScrollDirection = PointerEdgePositionToScrollDirection();
            PointerEdgePositionToScrollDirection();
            UpdateCameraPosition(mapScrollDirection, cameraSettings.mouseMapScrollSpeedMultiplier,
                cameraSettings.mouseMapDragSensitivity);
        }

        Vector3 PointerEdgePositionToScrollDirection() {
            var pointerPosition = mouseInputData.pointerPosition;
            var scrollDirectionX = RescaleViewportMinMaxCoordinate(pointerPosition.x, -1, 1f);
            var scrollDirectionZ = RescaleViewportMinMaxCoordinate(pointerPosition.y, -1, 1f);
            return new Vector3(scrollDirectionX, 0, scrollDirectionZ).normalized;
        }

        float RescaleViewportMinMaxCoordinate(float oldVal, float newMin, float newMax) {
            return (oldVal) * (newMax - newMin) + newMin;
        }

        void UpdateCameraPosition(Vector3 deltaPosition, float multiplierSpeed, float sensitivity) {
            var currentPos = thisTransform.localPosition;
            var positionChangeMultiplier = multiplierSpeed * dt * sensitivity;
            var desiredPosChange = deltaPosition * positionChangeMultiplier;
            var newPos = currentPos + desiredPosChange;
            var smoothedPos =
                Vector3.Slerp(currentPos, newPos, cameraSettings.cameraMoveSmoothing);
            thisTransform.localPosition = smoothedPos;
        }
    }
}