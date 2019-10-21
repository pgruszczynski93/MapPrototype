using DefaultNamespace;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraMovementLimiter : MonoBehaviour
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] protected MOCameraMovementArea cameraMovementArea;
        [SerializeField] protected MOBorders movementBorders;

        bool initialised;
        float minY;
        float maxY;

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            movementBorders = cameraMovementArea.MapBorders;
        }

        void Start()
        {
            Initialise();
        }

        void AssignEvents()
        {
            MOEvents.OnLateUpdate += ClampCameraMovement;
            MOCameraZoom.OnMinMaxZoomCalculated += HandleOnMinMaxZoomCalculated;
        }

        void RemoveEvents()
        {
            MOEvents.OnLateUpdate -= ClampCameraMovement;
            MOCameraZoom.OnMinMaxZoomCalculated -= HandleOnMinMaxZoomCalculated;
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }
        
        void HandleOnMinMaxZoomCalculated(float minZoom, float maxZoom) {
            minY = minZoom;
            maxY = maxZoom;
        }
        
        protected void ClampCameraMovement()
        {
            var clampedLocalPos = cameraTransform.localPosition;
            clampedLocalPos.x = Mathf.Clamp(clampedLocalPos.x, movementBorders.left, movementBorders.right);
            clampedLocalPos.y = Mathf.Clamp(clampedLocalPos.y, minY, maxY);
            clampedLocalPos.z = Mathf.Clamp(clampedLocalPos.z, movementBorders.bottom, movementBorders.top);
            cameraTransform.localPosition = clampedLocalPos;
        }

    }
}