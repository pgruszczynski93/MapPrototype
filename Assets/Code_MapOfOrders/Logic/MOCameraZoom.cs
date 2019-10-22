using System;
using DefaultNamespace;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOCameraZoom : MOCameraMovement {
        public static event Action<float, float> OnMinMaxZoomCalculated;

        bool isZoomLimitReached;
        int scrollDeltaValue;
        float minZoom;
        float maxZoom;
        
        protected override void AssignEvents()
        {
            base.AssignEvents();
            MOEvents.OnZoom += TryToInvokeZoomMovement;
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            MOEvents.OnZoom -= TryToInvokeZoomMovement;
        }

        public int CurrentHeightStep =>
            Mathf.RoundToInt(thisTransform.localPosition.y / tweenSetup.tweenSettings.positionDeltaMultiplier);

        public void TryToInvokeZoomMovement(int scrollValue) {
            Debug.Log("Zoom");
            scrollDeltaValue = scrollValue;
            UpdatePosition(default);
        }

        public static void BroadcastOnMinMaxZoomCalculated(float minZoom, float maxZoom) {
            OnMinMaxZoomCalculated?.Invoke(minZoom, maxZoom);
        }

        protected override void Initialise() {
            base.Initialise();
            var localPosition = thisTransform.localPosition;
            var startHeight = localPosition.y;
            minZoom = startHeight - cameraSettings.maxZoomValue;
            maxZoom = startHeight + cameraSettings.maxZoomValue;
            BroadcastOnMinMaxZoomCalculated(minZoom, maxZoom);
        }

        protected override void UpdatePosition(Vector3 pointerPos) {
            var zoomSign = Mathf.Sign(scrollDeltaValue);
            var zoomDelta = tweenSetup.tweenSettings.positionDeltaMultiplier * scrollDeltaValue;
            var currentPos = thisTransform.localPosition;

            var positionAfterScroll = currentPos + zoomDelta * thisTransform.forward;

            if (IsScrolledPositionOutOfZoomingLimit(positionAfterScroll, zoomSign)) {
                if (isZoomLimitReached)
                    return;

                isZoomLimitReached = true;
                var yLimit = Mathf.Clamp(positionAfterScroll.y, minZoom, maxZoom);
                var clampedPositionAfterScroll =
                    new Vector3(positionAfterScroll.x, yLimit, positionAfterScroll.z);

                PlayTween(clampedPositionAfterScroll);
            }
            else {
                isZoomLimitReached = false;
                PlayTween(positionAfterScroll);
            }
        }


        bool IsScrolledPositionOutOfZoomingLimit(Vector3 positionAfterScroll, float zoomSign) {
            return positionAfterScroll.y <= minZoom && zoomSign > 0 ||
                   positionAfterScroll.y >= maxZoom && zoomSign < 0;
        }
    }
}