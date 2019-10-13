using System;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOMouseInput : MonoBehaviour
    {
        readonly Vector3 VectorZero = Vector3.zero;

        [SerializeField] Camera mainCamera;
        [SerializeField] MOMouseInputData inputData;

        bool initialised;
        int screenWidth;
        int screenHeight;

        Vector2 mouseMovementDimensions;
        
        Vector3 mousePosition;
        Vector3 lastMousePointerPosition;
        Vector3 mouseMovementDelta;

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        void Start()
        {
            Initialise();
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
            if (IsMouseInScreenCoords())
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

        bool IsMouseInScreenCoords()
        {
            return mousePosition.x > 0
                   && mousePosition.x < screenWidth
                   && mousePosition.y > 0
                   && mousePosition.y < screenHeight;
        }

        bool IsMousePresent()
        {
            return Input.mousePresent;
        }

        bool CanCollectMapScrollMovementInput()
        {
            return IsMousePresent() && !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed()
        {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}