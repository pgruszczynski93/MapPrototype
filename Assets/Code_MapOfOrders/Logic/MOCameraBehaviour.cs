using Code_MapOfOrders.Logic;
using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace {
    public class MOCameraBehaviour : MonoBehaviour {
        bool isSelected;
        bool initialised;

        [SerializeField] Camera mapOrderCamera;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] MOCameraDrag cameraDrag;
        [SerializeField] MOCameraScroll cameraScroll;
        [SerializeField] MOCameraZoom cameraZoom;
        [SerializeField] MOHouseSelector houseSelector;

        MOMouseInputData inputData;

        void Initialise() {
            DOTween.Init();
            cameraDrag.SetupMovement(cameraSetup, mapOrderCamera);
            cameraScroll.SetupMovement(cameraSetup, mapOrderCamera);
            cameraZoom.SetupMovement(cameraSetup, mapOrderCamera);
            houseSelector.SetupHouseSelector(mapOrderCamera);
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
            MOEvents.OnMouseInputCollected += HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate += HandleMouseMovementActions;
        }

        void RemoveEvents() {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseMovementActions;
        }

        void HandleMouseInputCollectedReceived(MOMouseInputData mouseInputData) {
            inputData = mouseInputData;
        }

        void HandleMouseMovementActions() {
            switch (inputData.mouseAction) {
                case MouseAction.Undefined:
                    houseSelector.TryToSelectObject(inputData.pointerPosition, SelectionType.Highlight);
                    break;
                case MouseAction.MapSelection:
                    houseSelector.TryToSelectObject(inputData.pointerPosition, SelectionType.Selection);
                    break;
                case MouseAction.MapDragMovement:
                    cameraScroll.PauseTween();
                    cameraDrag.TryToInvokeDragMovement(inputData.pointerActionPosition);
                    break;
                case MouseAction.MapScrollMovement:
                    cameraDrag.PauseTween();
//                    cameraScroll.TryToInvokeScrollMovement(mouseInputData.pointerActionPosition);
                    break;
            }

            cameraZoom.TryToInvokeZoomMovement(inputData.scrollValue);
        }
    }
}