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

        Vector3 mouseDragStartPos;
        Vector3 mouseDragDelta;

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
            MOEvents.OnUpdate += TryToFetchMouseActions;
        }

        void RemoveEvents() {
            MOEvents.OnUpdate -= TryToFetchMouseActions;
        }

        void TryToFetchMouseActions() {
            if (!IsMousePresent())
                return;

            UpdateMouseViewportPosition();
            TryToSetScrollMapInputWhenCursorIsOutOfViewport();
            TryToFetchScrollButtonData();
            TryToSetDragMapInput();
        }
        
        void UpdateMouseViewportPosition() {

            mousePosition = Input.mousePosition;
            currentMouseViewportPos = mainCamera.ScreenToViewportPoint(mousePosition);
        }

        void TryToFetchScrollButtonData() {
            inputData.scrollValue = Input.mouseScrollDelta.y;
        }

        void TryToSetScrollMapInputWhenCursorIsOutOfViewport() {

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
            
            if (Input.GetMouseButtonDown(0)) {
                mouseDragStartPos = mousePosition;
                return;
            }
            
            if (!Input.GetMouseButton(0) || mouseDragStartPos == mousePosition) {
                ResetMouseActions();
                return;
            }
            
            inputData.mouseAction = MouseAction.MapDragMovement;
            
            mouseDragDelta = mainCamera.ScreenToViewportPoint(mouseDragStartPos - mousePosition);
            inputData.pointerPosition = mouseDragDelta;

            mouseDragStartPos = mousePosition;
            
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