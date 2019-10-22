using System;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOMouseInput : MonoBehaviour
    {
        readonly Vector3 VectorZero = Vector3.zero;

        [Range(1, 10), SerializeField] float screenBorder;
        [SerializeField] Camera mainCamera;

        bool initialised;

        Vector2 mouseMovementDimensions;
        Vector2 mouseMovementBorders;

        Vector3 mousePosition;
        Vector3 lastMousePointerPosition;
        Vector3 mouseMovementDelta;

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            mouseMovementBorders = new Vector3(Screen.width - screenBorder, Screen.height - screenBorder);
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

            mousePosition = Input.mousePosition;

            TryToBroadcastSelection();
            TryToBroadcastScroll();
            TryToBroadcastDrag();
            TryToBroadcastZoom();
            BroadcastPointerPositionUpdate();
        }

        void BroadcastPointerPositionUpdate()
        {
            MOEvents.BroadcastOnPointerPositionUpdate(mousePosition);
        }

        void TryToBroadcastZoom()
        {
            var scrollDelta = (int) Input.mouseScrollDelta.y;
            if (scrollDelta == 0)
                return;

            MOEvents.BroadcastOnZoom(scrollDelta);
        }

        void TryToBroadcastSelection()
        {
            if (!IsSelectionButtonPressed())
                return;

            MOEvents.BroadcastOnSelect(mousePosition);
        }

        void TryToBroadcastScroll()
        {
            if (!CanCollectMapScrollMovementInput() || IsMouseInScreenCoords())
                return;

            MOEvents.BroadcastOnScroll(mainCamera.ScreenToViewportPoint(mousePosition));
        }

        void TryToBroadcastDrag()
        {
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePointerPosition = mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1) || lastMousePointerPosition == mousePosition)
                return;

            mouseMovementDelta = mainCamera.ScreenToViewportPoint(lastMousePointerPosition - mousePosition);
            MOEvents.BroadcastOnDrag(mouseMovementDelta);
            lastMousePointerPosition = mousePosition;
        }

        bool IsMouseInScreenCoords()
        {
            return mousePosition.x > 0
                   && mousePosition.x < mouseMovementBorders.x
                   && mousePosition.y > 0
                   && mousePosition.y < mouseMovementBorders.y;
        }

        bool IsMousePresent()
        {
            return Input.mousePresent;
        }

        bool IsSelectionButtonPressed()
        {
            return Input.GetMouseButtonDown(0);
        }

        bool IsSelectionButtonReleased()
        {
            return Input.GetMouseButtonUp(1);
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