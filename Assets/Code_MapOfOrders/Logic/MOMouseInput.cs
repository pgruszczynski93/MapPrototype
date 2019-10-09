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
        
        Vector3 currentMouseViewportPos;
        Vector3 lastMouseOutOfViewportPos;
        Vector3 mouseOutOfViewportPosDelta;

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
            MOEvents.OnUpdate += TryToScrollMapWhenCursorIsOutOfViewport;
            MOEvents.OnUpdate += TryToFetchScrollButtonData;
        }

        void RemoveEvents() {
            MOEvents.OnUpdate -= TryToScrollMapWhenCursorIsOutOfViewport;
            MOEvents.OnUpdate -= TryToFetchScrollButtonData;
        }

        void TryToFetchScrollButtonData() {
            if (!IsMousePresent())
                return;

            inputData.scrollValue = Input.mouseScrollDelta.y;
        }

        void TryToScrollMapWhenCursorIsOutOfViewport() {
            //todo: change to REWIRED...
            if (!CanCollectMovementInput())
                return;
            
            var mousePosition = Input.mousePosition;
            currentMouseViewportPos = mainCamera.ScreenToViewportPoint(mousePosition);

            if (!IsMouseInViewport()) {
                mouseOutOfViewportPosDelta = currentMouseViewportPos - lastMouseOutOfViewportPos;
                inputData.mouseAction = MouseAction.MapScroll;
                inputData.pointerPositionDelta = mouseOutOfViewportPosDelta;
                lastMouseOutOfViewportPos = currentMouseViewportPos;
            }
            else {
                inputData.mouseAction = MouseAction.Stopped;
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
        
        bool CanCollectMovementInput() {
            return IsMousePresent() || !IsAnyNonScrollButtonPressed();
        }

        bool IsAnyNonScrollButtonPressed() {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1);
        }
    }
}