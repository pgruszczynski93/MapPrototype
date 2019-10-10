using System;
using System.Numerics;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Code_MapOfOrders.Logic {
    public class MOMouseInput : MonoBehaviour {
        readonly Vector3 VectorZero = Vector3.zero;

        [SerializeField] MOMouseInputSetup inputSetup;
        [SerializeField] MOMouseInputSettings inputSettings;
        [SerializeField] Camera mainCamera;
        [SerializeField] MOMouseInputData inputData;

        bool initialised;

        Vector3 mousePosition;
        Vector3 currentMouseViewportPos;
        Vector3 lastMouseOutOfViewportPos;
        Vector3 mouseOutOfViewportPosDelta;

        Vector3 mouseDragStartViewportPos;
        Vector3 mouseDragCurrentViewportPos;
        Vector3 mouseDragViewportDelta;

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            LoadAndApplySettings();
        }

        void Start() {
            Initialise();
        }

        void LoadAndApplySettings() {
            inputSettings = inputSetup.mouseInputSettings;
            //todo: more!!
        }

        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            MOEvents.OnUpdate += UpdateMouseViewportPosition;
            MOEvents.OnUpdate += TryToSetScrollMapInputWhenCursorIsOutOfViewport;
            MOEvents.OnUpdate += TryToFetchScrollButtonData;
            MOEvents.OnUpdate += TryToSetDragMapInput;
        }

        void RemoveEvents() {
            MOEvents.OnUpdate -= UpdateMouseViewportPosition;
            MOEvents.OnUpdate -= TryToSetScrollMapInputWhenCursorIsOutOfViewport;
            MOEvents.OnUpdate -= TryToFetchScrollButtonData;
            MOEvents.OnUpdate -= TryToSetDragMapInput;
        }

        void TryToFetchScrollButtonData() {
            if (!IsMousePresent())
                return;

            inputData.scrollValue = Input.mouseScrollDelta.y;
        }

        void UpdateMouseViewportPosition() {
            //todo: change to REWIRED...
            if (!IsMousePresent())
                return;

            mousePosition = Input.mousePosition;
            currentMouseViewportPos = mainCamera.ScreenToViewportPoint(mousePosition);
        }


        void TryToSetScrollMapInputWhenCursorIsOutOfViewport() {
            if (!CanCollectMapScrollMovementInput())
                return;

            InvokeMousePositionDependentAction(
                ResetMouseActions,
                SetScrollMapProperties);

            MOEvents.BroadcastOnMouseInput(inputData);
        }

        void ResetMouseActions() {
            inputData.mouseAction = MouseAction.Stopped;
        }

        void SetScrollMapProperties() {
            mouseOutOfViewportPosDelta = currentMouseViewportPos - lastMouseOutOfViewportPos;
            inputData.mouseAction = MouseAction.MapScrollMovement;
            inputData.pointerPosition = mouseOutOfViewportPosDelta;
            lastMouseOutOfViewportPos = currentMouseViewportPos;
        }

        void InvokeMousePositionDependentAction(Action onMouseInViewport, Action onMouseOutOfViewport) {
            if (IsMouseInViewport())
                onMouseInViewport?.Invoke();
            else
                onMouseOutOfViewport?.Invoke();
        }

        void TryToSetDragMapInput() {
            if (!IsMousePresent())
                return;

            if (Input.GetMouseButtonDown(1)) {
                inputData.mouseAction = MouseAction.MapDragMovement;
                mouseDragStartViewportPos = mainCamera.ScreenToViewportPoint(mousePosition);
            }
            else if (Input.GetMouseButton(1)) {
//                mouseOutOfViewportPosDelta = currentMouseViewportPos - lastMouseOutOfViewportPos;
                mouseDragCurrentViewportPos = mainCamera.ScreenToWorldPoint(mousePosition);
                inputData.mouseAction = MouseAction.MapDragMovement;
                mouseDragViewportDelta = currentMouseViewportPos - mouseDragStartViewportPos;
                inputData.pointerPosition = mouseDragViewportDelta;
//                lastMouseOutOfViewportPos = currentMouseViewportPos;
            }
            else if (Input.GetMouseButtonUp(1)) {
                ResetMouseActions();
            }
            
            MOEvents.BroadcastOnMouseInput(inputData);
        }

        bool IsMouseInViewport() {
            return currentMouseViewportPos.x > inputSettings.minViewportValueScrollAction
                   && currentMouseViewportPos.x < inputSettings.maxViewportValueScrollAction
                   && currentMouseViewportPos.y > inputSettings.minViewportValueScrollAction
                   && currentMouseViewportPos.y < inputSettings.maxViewportValueScrollAction;
        }

        bool IsMousePresent() {
            return Input.mousePresent;
        }

        bool CanCollectMapScrollMovementInput() {
            return IsMousePresent() && !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed() {
//            Debug.Log($"0: {Input.GetMouseButton(0)} 1: {Input.GetMouseButton(1)}");
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}