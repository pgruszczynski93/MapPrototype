using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOMouseInput : MonoBehaviour {
        [SerializeField] Camera mainCamera;
        [SerializeField] MOMouseInputData inputData;
        [SerializeField] MouseAction mouseAction;


        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            MOEvents.OnUpdate += TryToScrollMapWhenCursorIsOutOfViewport;
        }

        void RemoveEvents() {
            MOEvents.OnUpdate -= TryToScrollMapWhenCursorIsOutOfViewport;
        }

        void TryToScrollMapWhenCursorIsOutOfViewport() {
            //todo: change to REWIRED...
            if (!CanScrollMap())
                return;

            mouseAction = MouseAction.Scroll;
            inputData.scrollValue = Input.mouseScrollDelta.y;
            inputData.mouseCoordinates = new Vector2(0, 0);
            MOEvents.BroadcastOnMouseInput(inputData);
        }

        bool CanScrollMap() {
            return CanCollectMovementInput() && IsMouseInViewport();
        }
        
        bool IsMouseInViewport() {
            var cameraViewportPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);

            return cameraViewportPos.x >= MOConsts.VIEWPORT_MIN
                   && cameraViewportPos.x <= MOConsts.VIEWPORT_MAX
                   && cameraViewportPos.y >= MOConsts.VIEWPORT_MIN
                   && cameraViewportPos.y <= MOConsts.VIEWPORT_MAX;
        }

        bool CanCollectMovementInput() {
            return Input.mousePresent || !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed() {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}