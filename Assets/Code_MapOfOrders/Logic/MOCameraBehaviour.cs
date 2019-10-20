using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace
{
    public class MOCameraBehaviour : MonoBehaviour
    {
        bool isZooming;
        bool isZoomLimitReached;
        bool isSelected;
        bool initialised;

        float minZoom;
        float maxZoom;
        float currentZoom;

        [SerializeField] Camera mapOrderCamera;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] MOCameraDrag cameraDrag;
        [SerializeField] MOCameraZoom cameraZoom;
        [SerializeField] MOCameraScroll cameraScroll;
//        [SerializeField] protected MOHouseSelector houseSelector;

        MOMouseInputData mouseInputData;

        void Initialise()
        {
            cameraDrag.SetupMovement(cameraSetup, mapOrderCamera);
            cameraScroll.SetupMovement(cameraSetup, mapOrderCamera);
//            cameraZoom.SetupMovement(cameraSetup, mapOrderCamera);
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
            MOEvents.OnMouseInputCollected += HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate += HandleMouseMovementActions;
        }

        void RemoveEvents()
        {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
            MOEvents.OnLateUpdate -= HandleMouseMovementActions;
        }
        
        void HandleMouseInputCollectedReceived(MOMouseInputData inputData)
        {
            mouseInputData = inputData;
        }

        void HandleMouseMovementActions()
        {
            switch (mouseInputData.mouseAction)
            {
                case MouseAction.Undefined:
                    //ResetSelectingPossibility();
                    break;
                case MouseAction.MapSelection:
                    //TryToSelect();
                    break;
                case MouseAction.MapDragMovement:
                    cameraDrag.TryToInvokeDragMovement(mouseInputData.pointerActionPosition);
                    break;
                case MouseAction.MapScrollMovement:
                    cameraScroll.TryToInvokeScrollMovement(mouseInputData.pointerActionPosition);
                    break;
                default:
                    break;
            }

            if (isZooming == false)
            {
//                TryToZoomMap();
            }

//            ClampCameraMovement();
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

//        void TryToZoomMap()
//        {
//            var scrollValue = mouseInputData.scrollValue;
//            if (scrollValue == 0 || isZooming)
//                return;
//
//            Debug.Log("[MOCameraMovement] Zoom");
//            var zoomSign = Mathf.Sign(scrollValue);
//            var zoomDelta = cameraSetup.cameraSettings.zoomTweenProperties.positionDeltaMultiplier * zoomSign;
//            var currentPos = thisTransform.localPosition;
//            var nextZoom = currentZoom - zoomDelta;
//
//            var positionAfterScroll = currentPos + zoomDelta * thisTransform.forward;
//
////            Debug.Log("B scroll " + positionAfterScroll.y);
//            if (IsOutOfZoomingLimit(positionAfterScroll, zoomSign))
//            {
//                if (isZoomLimitReached)
//                    return;
//                var afterScrollY = Mathf.Clamp(positionAfterScroll.y, minZoom, maxZoom);
//                var clampedPositionAfterScroll =
//                    new Vector3(positionAfterScroll.x, afterScrollY, positionAfterScroll.z);
//
//                thisTransform
//                    .DOMove(clampedPositionAfterScroll, cameraSetup.cameraSettings.zoomTweenProperties.tweenTime)
//                    .SetEase(cameraSetup.cameraSettings.zoomTweenProperties.easeType)
//                    .OnComplete(() => { currentZoom = Mathf.Clamp(nextZoom, minZoom, maxZoom); });
//
//                isZoomLimitReached = true;
//            }
//            else
//            {
//                thisTransform
//                    .DOMove(positionAfterScroll, cameraSetup.cameraSettings.zoomTweenProperties.tweenTime)
//                    .SetEase(cameraSetup.cameraSettings.zoomTweenProperties.easeType)
//                    .OnComplete(() => { currentZoom = Mathf.Clamp(nextZoom, minZoom, maxZoom); });
//                isZoomLimitReached = false;
//            }
//        }

//        private bool IsOutOfZoomingLimit(Vector3 positionAfterScroll, float zoomSign)
//        {
//            return positionAfterScroll.y <= minZoom && zoomSign > 0 ||
//                   positionAfterScroll.y >= maxZoom && zoomSign < 0;
//        }
//
//        void TryToInvokeScrollMapMovement()
//        {
//            if (mouseInputData.mouseAction != MouseAction.MapScrollMovement)
//                thisTransform.DOPause();
//            else
//            {
//                Debug.Log("[MOCameraMovement] Scroll map");
//                var mapScrollDirection = PointerEdgePositionToScrollDirection();
//                PointerEdgePositionToScrollDirection();
//                UpdateCameraPosition(mapScrollDirection, cameraSetup.cameraSettings.scrollTweenProperties);
//            }
//        }
//
    }
}