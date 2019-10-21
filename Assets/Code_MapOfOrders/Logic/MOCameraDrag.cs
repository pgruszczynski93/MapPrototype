﻿using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOCameraDrag : MOCameraMovement {

        [SerializeField] MOCameraZoom cameraZoom;
        
        Vector3 dragPosition;
        Vector3 newDragPosition;

        public void TryToInvokeDragMovement(Vector3 pointerPos) {
            dragPosition = new Vector3(pointerPos.x, 0, pointerPos.y);
            UpdatePosition();
        }

        protected override void UpdatePosition() {
            var currentPos = thisTransform.localPosition;
//            Debug.Log("XD "+(cameraZoom.CurrentZoomStep
//                             * tweenSetup.tweenSettings.positionDeltaMultiplier));
            var desiredPosChange = dragPosition
                                   * cameraZoom.CurrentZoomStep
                                   * tweenSetup.tweenSettings.positionDeltaMultiplier ;
            newDragPosition = currentPos + desiredPosChange;
            PlayTween(newDragPosition);
        }
    }
}