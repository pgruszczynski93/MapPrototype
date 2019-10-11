using System;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOMouseInput : MonoBehaviour {
        readonly Vector3 VectorZero = Vector3.zero;

        [SerializeField] MOMouseInputSetup inputSetup;
        [SerializeField] MOMouseInputSettings inputSettings;
        [SerializeField] Camera mainCamera;
        [SerializeField] MOMouseInputData inputData;

        bool initialised;

        Vector2 screenDimensions;
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
            screenDimensions = new Vector2(Screen.width, Screen.height);
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
        }

        void TryToFetchScrollButtonData() {
            inputData.scrollValue = Input.mouseScrollDelta.y;
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
//            if()
            if (IsMouseInScreenBoundaries())
                onMouseInViewport?.Invoke();
            else
                onMouseOutOfViewport?.Invoke();
        }
        
        void TryToSetDragMapInput() {
            
            if (Input.GetMouseButtonDown(1)) {
                mouseDragStartPos = mousePosition;
                return;
            }
            
            if (!Input.GetMouseButton(1) || mouseDragStartPos == mousePosition) {
                ResetMouseActions();
                inputData.pointerPosition = VectorZero;
                MOEvents.BroadcastOnMouseInput(inputData);
                return;
            }
            
            inputData.mouseAction = MouseAction.MapDragMovement;
            mouseDragDelta = mainCamera.ScreenToViewportPoint(mouseDragStartPos - mousePosition);
            inputData.pointerPosition = mouseDragDelta;
            mouseDragStartPos = mousePosition;
            
            MOEvents.BroadcastOnMouseInput(inputData);
        }

        bool IsMouseInScreenBoundaries() {
            return mousePosition.x >= inputSettings.edgeThickness
                   && mousePosition.x < screenDimensions.x
                   && mousePosition.y >= inputSettings.edgeThickness
                   && mousePosition.y < screenDimensions.y;
        }

        bool IsMousePresent() {
            return Input.mousePresent;
        }

        bool CanCollectMapScrollMovementInput() {
            return IsMousePresent() && !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed() {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}