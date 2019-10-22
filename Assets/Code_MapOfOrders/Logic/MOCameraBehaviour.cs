using Code_MapOfOrders.Logic;
using DG.Tweening;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace {
    public class MOCameraBehaviour : MonoBehaviour {
        bool initialised;

        [SerializeField] Camera mapOrderCamera;
        [SerializeField] MOCameraSetup cameraSetup;

        [SerializeField] MOCameraDrag cameraDrag;
        [SerializeField] MOCameraScroll cameraScroll;
        [SerializeField] MOCameraZoom cameraZoom;
        [SerializeField] MOHouseSelector houseSelector;

        void Initialise() {
            DOTween.Init();
            cameraDrag.SetupMovement(cameraSetup, mapOrderCamera);
            cameraScroll.SetupMovement(cameraSetup, mapOrderCamera);
            cameraZoom.SetupMovement(cameraSetup, mapOrderCamera);
            houseSelector.SetupHouseSelector(mapOrderCamera);
        }

        void Start() {
            Initialise();
        }
    }
}