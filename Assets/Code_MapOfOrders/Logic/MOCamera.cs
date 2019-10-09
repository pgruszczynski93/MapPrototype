using System;
using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace {
    [RequireComponent(typeof(Camera))]
    public class MOCamera : MonoBehaviour {
        [SerializeField] MOCameraSettings cameraSettings;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] Camera assignedCamera;
        [SerializeField] Transform thisTransform;

        bool initialised;

        float lastPointerMovementMagnitude;
        Vector3 lastPointerTranslation;

        void Start() {
            Initialise();
        }

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            LoadSettings();
            ApplySettings();
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

        void LoadSettings() {
            cameraSettings = cameraSetup.cameraSettings;
        }

        void ApplySettings() {
            assignedCamera.fieldOfView = cameraSettings.cameraFov;
            //todo: more!!
        }
        
        void HandleMouseInputCollectedReceived(MOMouseInputData inputData) {
            HandleMouseAction(inputData);
        }

        void HandleMouseAction(MOMouseInputData inputData) {
            switch (inputData.mouseAction) {
                case MouseAction.Stopped:
                    break;
                case MouseAction.Selection:
                    break;
                case MouseAction.Movement:
                    break;
                case MouseAction.Scroll:
                    MoveCameraOnScroll(inputData.pointerPositionDelta);
                    break;
                default:
                    break;
            }
        }

        void MoveCameraOnScroll(Vector3 pointerPosDelta) {
            if (pointerPosDelta.magnitude > 0)
                SetLastPointerOutOfViewportTranslation(pointerPosDelta);
            
            SetCameraPositionWhenScrolling(pointerPosDelta);
        }

        void SetLastPointerOutOfViewportTranslation(Vector3 pointerPosDelta) {
            lastPointerTranslation = new Vector3(pointerPosDelta.x, 0f, pointerPosDelta.y).normalized;
        }

        void SetCameraPositionWhenScrolling(Vector3 pointerPosDelta) {
            var currentPosition = thisTransform.localPosition;
            var newPosition = currentPosition + lastPointerTranslation;
            var smoothedPosition =
                Vector3.Slerp(currentPosition, newPosition, Time.deltaTime * cameraSettings.mouseScrollSpeedScaller);
            thisTransform.localPosition = new Vector3(smoothedPosition.x, currentPosition.y, smoothedPosition.z);
        }
    }
}