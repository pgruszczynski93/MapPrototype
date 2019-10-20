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
        [SerializeField] MOMouseInputData inputData;

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

            inputData.pointerPosition = mousePosition;
            
            TryToSetSelectInput();
            TryToSetScrollInput();
            TryToSetDragInput();
            
            MOEvents.BroadcastOnMouseInput(inputData);
        }

        void TryToSetSelectInput() {
            if (!IsSelectionButtonPressed()) 
                return;
            
            inputData.pointerActionPosition = mousePosition;
            inputData.mouseAction = MouseAction.MapSelection;
           

        }

        void TryToSetScrollInput()
        {
            if (!CanCollectMapScrollMovementInput())
                return;

            inputData.scrollValue = (int)Input.mouseScrollDelta.y;

            mouseMovementDelta = mainCamera.ScreenToViewportPoint(mousePosition - lastMousePointerPosition);

            RecognizeScreenEdgeAction(()=> {
                    ResetMouseActions();
                    inputData.pointerActionPosition = mainCamera.ScreenToViewportPoint(mousePosition);

                },
                SetScrollMapProperties);
        }

        void ResetMouseActions()
        {
            inputData.mouseAction = MouseAction.Undefined;
        }

        void SetScrollMapProperties()
        {
            inputData.mouseAction = MouseAction.MapScrollMovement;
            inputData.pointerActionPosition = mainCamera.ScreenToViewportPoint(mousePosition);
        }

        void RecognizeScreenEdgeAction(Action onMouseInViewport, Action onMouseOutOfViewport)
        {
            if (IsMouseInScreenCoords() && !IsSelectionButtonPressed())
                onMouseInViewport?.Invoke();
            else
                onMouseOutOfViewport?.Invoke();
        }

        void TryToSetDragInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePointerPosition = mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1) || lastMousePointerPosition == mousePosition)
            {
                MOEvents.BroadcastOnMouseInput(inputData);
                return;
            }

            inputData.mouseAction = MouseAction.MapDragMovement;
            mouseMovementDelta = mainCamera.ScreenToViewportPoint(lastMousePointerPosition - mousePosition);
            inputData.pointerActionPosition = mouseMovementDelta;
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

        bool IsSelectionButtonPressed() {
            return Input.GetMouseButtonDown(0);
        }

        bool IsSelectionButtonReleased() {
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