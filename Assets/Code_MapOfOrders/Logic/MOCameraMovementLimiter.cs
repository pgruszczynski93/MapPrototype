using DefaultNamespace;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOCameraMovementLimiter : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] protected MOCameraMovementArea cameraMovementArea;
        [SerializeField] protected MOBorders movementBorders;

        bool initialised;

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
        }

        void RemoveEvents()
        {
            MOEvents.OnLateUpdate -= ClampCameraMovement;
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }
        
        protected void ClampCameraMovement()
        {
            var clampedLocalPos = cameraTransform.localPosition;
            clampedLocalPos.x = Mathf.Clamp(clampedLocalPos.x, movementBorders.left, movementBorders.right);
//            clampedLocalPos.y = Mathf.Clamp(clampedLocalPos.y, minZoom, maxZoom);
            clampedLocalPos.z = Mathf.Clamp(clampedLocalPos.z, movementBorders.bottom, movementBorders.top);
            cameraTransform.localPosition = clampedLocalPos;
        }

    }
}