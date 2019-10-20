using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraDrag : MOCameraMovement
    {
        Vector3 dragPosition;

        public void TryToInvokeDragMovement(Vector3 pointerPos)
        {
            Debug.Log("[MOCameraMovement] Drag");
            dragPosition = new Vector3(pointerPos.x, 0, pointerPos.y);
            UpdatePosition();
        }

        protected override void UpdatePosition()
        {
            var currentPos = thisTransform.localPosition;
            var desiredPosChange = dragPosition * tweenSetup.tweenSettings.positionDeltaMultiplier;
            var newPos = currentPos + desiredPosChange;
            thisTransform.DOLocalMove(newPos, tweenSetup.tweenSettings.tweenTime)
                .SetEase(tweenSetup.tweenSettings.easeType);
        }
    }
}