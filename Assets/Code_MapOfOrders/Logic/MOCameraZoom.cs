using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraZoom : MOCameraMovement
    {
        float minZoom;
        float maxZoom;
        float currentZoom;
        protected override void Initialise()
        {
            base.Initialise();
            var startHeight = thisTransform.localPosition.y;
            currentZoom = startHeight;
            minZoom = startHeight - cameraSettings.maxZoomValue;
            maxZoom = startHeight + cameraSettings.maxZoomValue;
        }

        protected override void UpdatePosition()
        {
            
        }
    }
}