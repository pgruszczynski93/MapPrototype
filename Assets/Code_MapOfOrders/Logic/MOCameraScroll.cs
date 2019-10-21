using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraScroll : MOCameraMovement
    {
        Vector3 scaledViewportPosition;
        Vector3 newScrolledPosition;

        public void TryToInvokeScrollMovement(Vector3 pointerPos)
        {
            scaledViewportPosition = PointerEdgePositionToScrollDirection(pointerPos);
            UpdatePosition();
        }

        protected override void UpdatePosition()
        {
            var currentPos = thisTransform.localPosition;
            var desiredPosChange = scaledViewportPosition * tweenSetup.tweenSettings.positionDeltaMultiplier;
            newScrolledPosition = currentPos + desiredPosChange;
            PlayTween(newScrolledPosition);
        }

        float RescaleViewportMinMaxCoordinate(float oldVal, float newMin, float newMax)
        {
            return oldVal * (newMax - newMin) + newMin;
        }

        Vector3 PointerEdgePositionToScrollDirection(Vector3 pointerPos)
        {
            var pointerPosition = pointerPos;
            var scrollDirectionX = RescaleViewportMinMaxCoordinate(pointerPosition.x, -1, 1f);
            var scrollDirectionZ = RescaleViewportMinMaxCoordinate(pointerPosition.y, -1, 1f);
            return new Vector3(scrollDirectionX, 0, scrollDirectionZ).normalized;
        }
    }
}