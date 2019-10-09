using System.Numerics;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Code_MapOfOrders.Logic {
    public class MOMouseInput : MonoBehaviour {
        readonly Vector3 VectorZero = Vector3.zero;
        
        [SerializeField] Camera mainCamera;
        [SerializeField] MOMouseInputData inputData;

        Vector3 currentMouseViewportPos;
        Vector3 lastMouseOutOfViewportPos;
        Vector3 mouseOutOfViewportPosDelta;
        
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
            if (!CanCollectMovementInput())
                return;
            
            var mousePosition = Input.mousePosition;
            currentMouseViewportPos = mainCamera.ScreenToViewportPoint(mousePosition);

            if (!IsMouseInViewport()) {
                mouseOutOfViewportPosDelta = currentMouseViewportPos - lastMouseOutOfViewportPos;
                inputData.mouseAction = MouseAction.Scroll;
                inputData.scrollValue = Input.mouseScrollDelta.y;
                inputData.pointerPositionDelta = mouseOutOfViewportPosDelta;
                lastMouseOutOfViewportPos = currentMouseViewportPos;
            }
            else {
                inputData.mouseAction = MouseAction.Stopped;
            }
            
            MOEvents.BroadcastOnMouseInput(inputData);
        }

        bool IsMouseInViewport() {

            return currentMouseViewportPos.x > MOConsts.VIEWPORT_MIN
                   && currentMouseViewportPos.x < MOConsts.VIEWPORT_MAX
                   && currentMouseViewportPos.y > MOConsts.VIEWPORT_MIN
                   && currentMouseViewportPos.y < MOConsts.VIEWPORT_MAX;
        }

        bool CanCollectMovementInput() {
            return Input.mousePresent || !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed() {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}