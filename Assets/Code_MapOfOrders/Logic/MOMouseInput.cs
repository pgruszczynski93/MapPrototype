using System;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOMouseInput : MonoBehaviour
    {
        readonly Vector3 VectorZero = Vector3.zero;

        [SerializeField] MOMouseInputSetup inputSetup;
        [SerializeField] MOMouseInputSettings inputSettings;
        [SerializeField] Camera mainCamera;
        [SerializeField] MOMouseInputData inputData;

        bool initialised;

        Vector2 mouseMovementDimensions;

        Vector3 mousePosition;
        Vector3 lastMousePointerPosition;
        Vector3 mouseMovementDelta;

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            LoadAndApplySettings();
        }

        void Start()
        {
            Initialise();
        }

        void LoadAndApplySettings()
        {
            inputSettings = inputSetup.mouseInputSettings;
            mouseMovementDimensions = new Vector2(Screen.width - inputSettings.edgeThickness,
                Screen.height - inputSettings.edgeThickness);

            //todo: more!!
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
            MOEvents.OnUpdate += TryToFetchMouseActions;
        }

        void RemoveEvents()
        {
            MOEvents.OnUpdate -= TryToFetchMouseActions;
        }

        void TryToFetchMouseActions()
        {
            if (!IsMousePresent())
                return;

            UpdateMousePosition();
            TryToSetScrollMapInputWhenCursorIsOutOfViewport();
            TryToFetchScrollButtonData();
            TryToSetDragMapInput();
        }

        void UpdateMousePosition()
        {
            mousePosition = Input.mousePosition;
        }

        void TryToFetchScrollButtonData()
        {
            inputData.scrollValue = Input.mouseScrollDelta.y;
        }

        void TryToSetScrollMapInputWhenCursorIsOutOfViewport()
        {
            if (!CanCollectMapScrollMovementInput())
                return;

            mouseMovementDelta = mainCamera.ScreenToViewportPoint(mousePosition - lastMousePointerPosition);

            RecognizeScreenEdgeAction(ResetMouseActions,
                SetScrollMapProperties);

            MOEvents.BroadcastOnMouseInput(inputData);
        }

        void ResetMouseActions()
        {
            inputData.mouseAction = MouseAction.Stopped;
        }

        void SetScrollMapProperties()
        {
            inputData.mouseAction = MouseAction.MapScrollMovement;
            inputData.pointerPosition = mainCamera.ScreenToViewportPoint(mousePosition);
        }

        void RecognizeScreenEdgeAction(Action onMouseInViewport, Action onMouseOutOfViewport)
        {
            if (IsMouseInScreenBoundaries())
                onMouseInViewport?.Invoke();
            else
                onMouseOutOfViewport?.Invoke();
        }

        void TryToSetDragMapInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePointerPosition = mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1) || lastMousePointerPosition == mousePosition)
            {
                if (inputData.mouseAction != MouseAction.MapScrollMovement)
                    ResetMouseActions();
                MOEvents.BroadcastOnMouseInput(inputData);
                return;
            }

            inputData.mouseAction = MouseAction.MapDragMovement;
            mouseMovementDelta = mainCamera.ScreenToViewportPoint(lastMousePointerPosition - mousePosition);
            inputData.pointerPosition = mouseMovementDelta;
            lastMousePointerPosition = mousePosition;

            MOEvents.BroadcastOnMouseInput(inputData);
        }

        bool IsMouseInScreenBoundaries()
        {
            return mousePosition.x >= inputSettings.edgeThickness
                   && mousePosition.x <= mouseMovementDimensions.x
                   && mousePosition.y >= inputSettings.edgeThickness
                   && mousePosition.y <= mouseMovementDimensions.y;
        }

        bool IsMousePresent()
        {
            return Input.mousePresent;
        }

        bool CanCollectMapScrollMovementInput()
        {
            return !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed()
        {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}