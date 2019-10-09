using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace {
    [RequireComponent(typeof(Camera))]
    public class MOCamera : MonoBehaviour {
        [SerializeField] MOCameraSettings cameraSettings;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] Camera assignedCamera;
        [SerializeField] Transform thisTransform;

        bool initialised;


        void Start() {
            Initialise();
        }

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            LoadSettings();
            ApplySettings();
        }

        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            
            MOEvents.OnMouseInputCollected += HandleMouseInputCollectedReceived;
        }

        void RemoveEvents() {
            MOEvents.OnMouseInputCollected -= HandleMouseInputCollectedReceived;
        }

        void LoadSettings() {
            cameraSettings = cameraSetup.cameraSettings;
        }

        void ApplySettings() {
            assignedCamera.fieldOfView = cameraSettings.cameraFov;
            //todo: more!!
        }
        
        void HandleMouseInputCollectedReceived(MOMouseInputData inputData) {
            
        }

    }
}