using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraScroll : MOCameraMovement
    {
        Vector3 zoomPosition;
        Vector3 newScrolledPosition;

        public void TryToInvokeScrollMovement(Vector3 pointerPos)
        {
            zoomPosition = PointerEdgePositionToScrollDirection(pointerPos);
            UpdatePosition();
        }

        protected override void UpdatePosition()
        {
            Debug.Log("[MOCameraMovement] Scroll");
            var currentPos = thisTransform.localPosition;
            var desiredPosChange = zoomPosition * tweenSetup.tweenSettings.positionDeltaMultiplier;
            newScrolledPosition = currentPos + desiredPosChange;
            tweenTargetPos = newScrolledPosition;
            PlayTween();
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