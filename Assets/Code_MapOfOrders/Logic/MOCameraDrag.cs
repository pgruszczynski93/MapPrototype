using DefaultNamespace;
using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOCameraDrag : MOCameraMovement {

        [SerializeField] MOCameraZoom cameraZoom;
        
        Vector3 dragPosition;
        Vector3 newDragPosition;

        protected override void AssignEvents()
        {
            base.AssignEvents();
            MOEvents.OnDrag += UpdatePosition;
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            MOEvents.OnDrag -= UpdatePosition;
        }
        protected override void UpdatePosition(Vector3 pointerPos) {
            Debug.Log("DRAG");
            dragPosition = new Vector3(pointerPos.x, 0, pointerPos.y);               
            var currentPos = thisTransform.localPosition;
            var desiredPosChange = tweenSetup.tweenSettings.positionDeltaMultiplier
                                   * cameraZoom.CurrentHeightStep
                                   * dragPosition;
            newDragPosition = currentPos + desiredPosChange;
            PlayTween(newDragPosition);
        }
    }
}