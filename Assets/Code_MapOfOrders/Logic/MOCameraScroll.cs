using DefaultNamespace;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraScroll : MOCameraMovement
    {
        Vector3 scaledViewportPosition;
        Vector3 newScrolledPosition;
        
        protected override void AssignEvents()
        {
            base.AssignEvents();
            MOEvents.OnScroll += UpdatePosition;
            MOEvents.OnDrag += PauseTween;
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            MOEvents.OnScroll -= UpdatePosition;
            MOEvents.OnDrag -= PauseTween;
        }
        
        protected override void UpdatePosition(Vector3 pointerPos)
        {
            scaledViewportPosition = PointerEdgePositionToScrollDirection(pointerPos);
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