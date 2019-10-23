using System;
using System.Threading;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOMouseInput : MonoBehaviour {
        readonly Vector3 VectorZero = Vector3.zero;

        [SerializeField] MOMouseInputSetup inputSetup;
        [SerializeField] Camera mainCamera;

        bool initialised;

        Vector2 mouseMovementDimensions;

        Vector3 mousePosition;
        Vector3 lastMousePointerPosition;
        Vector3 mouseMovementDelta;

        Vector2[] mouseMovementBorders;
        MOMouseInputSettings inputSettings;

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            if (inputSetup == null) {
                Debug.LogError("No input setup asset attached!");
                return;
            }
            inputSettings = inputSetup.mouseInputSettings;
            var width = Screen.width;
            var height = Screen.height;
            var horizontalViewportMin =
                Mathf.RoundToInt(width * (inputSettings.horizontalEdgeThicknessPercentage * MOConsts.PERCENTAGE));
            var horizontalViewportMax = width - horizontalViewportMin;
            var verticalViewportMin =
                Mathf.RoundToInt(height * (inputSettings.verticalEdgeThicknessPercentage * MOConsts.PERCENTAGE));
            var verticalViewportMax = height - verticalViewportMin;

            mouseMovementBorders = new[] {
                new Vector2(horizontalViewportMin, verticalViewportMin),
                new Vector2(horizontalViewportMax, verticalViewportMax),
            };
        }

        void Start() {
            Initialise();
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

            mousePosition = Input.mousePosition;

            TryToBroadcastSelection();
            TryToBroadcastScroll();
            TryToBroadcastDrag();
            TryToBroadcastZoom();
            TryToBroadcastExitMap();
        }

        void TryToBroadcastExitMap() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                MOEvents.BroadcastOnMapExit();
            }
        }

        void TryToBroadcastZoom() {
            var scrollDelta = (int) Input.mouseScrollDelta.y;
            if (scrollDelta == 0)
                return;

            MOEvents.BroadcastOnZoom(scrollDelta);
        }

        void TryToBroadcastSelection() {
            MOEvents.BroadcastOnSelect(mousePosition, MapSelectionType.Highlight);

            if (!IsSelectionButtonPressed())
                return;

            MOEvents.BroadcastOnSelect(mousePosition, MapSelectionType.Selection);
        }

        void TryToBroadcastScroll() {
            if (!CanCollectMapScrollMovementInput() || IsMouseInScreenCoords())
                return;

            MOEvents.BroadcastOnScroll(mainCamera.ScreenToViewportPoint(mousePosition));
        }

        void TryToBroadcastDrag() {
            if (Input.GetMouseButtonDown(1)) {
                lastMousePointerPosition = mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1) || lastMousePointerPosition == mousePosition)
                return;

            mouseMovementDelta = mainCamera.ScreenToViewportPoint(lastMousePointerPosition - mousePosition);
            MOEvents.BroadcastOnDrag(mouseMovementDelta);
            lastMousePointerPosition = mousePosition;
        }

        bool IsMouseInScreenCoords() {
            return mousePosition.x > mouseMovementBorders[0].x
                   && mousePosition.x < mouseMovementBorders[1].x
                   && mousePosition.y > mouseMovementBorders[0].y
                   && mousePosition.y < mouseMovementBorders[1].y;
        }

        bool IsMousePresent() {
            return Input.mousePresent;
        }

        bool IsSelectionButtonPressed() {
            return Input.GetMouseButtonDown(0);
        }

        bool IsSelectionButtonReleased() {
            return Input.GetMouseButtonUp(1);
        }

        bool CanCollectMapScrollMovementInput() {
            return IsMousePresent() && !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed() {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}