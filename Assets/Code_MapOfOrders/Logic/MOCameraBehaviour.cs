using Code_MapOfOrders.Logic;
using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace {
    public class MOCameraBehaviour : MonoBehaviour {
        
        bool isZoomLimitReached;
        bool isSelected;
        bool initialised;

        [SerializeField] Camera mapOrderCamera;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] MOCameraDrag cameraDrag;
        [SerializeField] MOCameraScroll cameraScroll;
        [SerializeField] MOCameraZoom cameraZoom;
//        [SerializeField] protected MOHouseSelector houseSelector;

        MOMouseInputData mouseInputData;

        void Initialise() {
            DOTween.Init();
            cameraDrag.SetupMovement(cameraSetup, mapOrderCamera);
            cameraScroll.SetupMovement(cameraSetup, mapOrderCamera);
            cameraZoom.SetupMovement(cameraSetup, mapOrderCamera);
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

        void HandleMouseInputCollectedReceived(MOMouseInputData inputData) {
            mouseInputData = inputData;
        }

        void HandleMouseMovementActions() {
            switch (mouseInputData.mouseAction) {
                case MouseAction.Undefined:
                    //ResetSelectingPossibility();
                    break;
                case MouseAction.MapSelection:
                    //TryToSelect();
                    break;
                case MouseAction.MapDragMovement:
                    cameraScroll.PauseTween();
                    cameraDrag.TryToInvokeDragMovement(mouseInputData.pointerActionPosition);
                    break;
                case MouseAction.MapScrollMovement:
                    cameraDrag.PauseTween();
                    cameraScroll.TryToInvokeScrollMovement(mouseInputData.pointerActionPosition);
                    break;
            }
            cameraZoom.TryToInvokeZoomMovement(mouseInputData.scrollValue);
        }

//        void ResetSelectingPossibility()
//        {
//            isSelected = false;
//
//            houseSelector.TryToHighlightObject(thisCamera.ScreenPointToRay(mouseInputData.pointerPosition));
//        }
//
//        void TryToSelect()
//        {
//            if (isSelected)
//                return;
//            Debug.Log("[MOCameraMovement] Select");
//            isSelected = true;
//            houseSelector.Show(thisCamera.ScreenPointToRay(mouseInputData.pointerActionPosition));
//        }
//
    }
}