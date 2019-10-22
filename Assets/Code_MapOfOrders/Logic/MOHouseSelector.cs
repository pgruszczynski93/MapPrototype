using System.Collections.Generic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOHouseSelector : MonoBehaviour {
        const string HOUSE_TAG = "TestSelection";

        [SerializeField] MOMapOrderHouse[] mapOfOrdersHouses;
        [SerializeField] MOMapOrderHouse pointedHouse;
        [SerializeField] MOMapOrderHouse selectedHouse;

        bool initialised;
        Camera mapOrderCamera;
        Ray selectionRay;

        Dictionary<Transform, MOMapOrderHouse> housesCache;

        void Debug_ShowHousesState() {
            foreach (var h in mapOfOrdersHouses) {
                Debug.Log(h.name + " " + h.houseState);
            }
        }

//        void Update() {
//            Debug_ShowHousesState();
//        }

        void Initialise() {
            if (initialised)
                return;

            initialised = true;
            TryToInitialiseHousesCache();
        }

        void TryToInitialiseHousesCache() {
            if (mapOfOrdersHouses.Length == 0 || mapOfOrdersHouses == null) {
                Debug.LogError("No selectable houses assigned.");
                return;
            }

            housesCache = new Dictionary<Transform, MOMapOrderHouse>();
            for (var i = 0; i < mapOfOrdersHouses.Length; i++) {
                var currentHouse = mapOfOrdersHouses[i];
                housesCache.Add(currentHouse.transform, currentHouse);
            }
        }

        void Start() {
            Initialise();
        }

        public void SetupHouseSelector(Camera cam) {
            mapOrderCamera = cam;
        }

        public void TryToHighlightHouse(Vector3 pointerPos) {

//            if (selectedHouse) {
//                selectedHouse.ManageSelectedHouse(HouseAction.Selected);
//            }
            
            selectionRay = mapOrderCamera.ScreenPointToRay(pointerPos);
            RaycastHit hitInfo;

            if (!Physics.Raycast(selectionRay, out hitInfo))
                return;


            if (hitInfo.collider.CompareTag(HOUSE_TAG)) {
                var hitTransform = hitInfo.transform;

                if (!housesCache.TryGetValue(hitTransform, out var selection))
                    return;

                pointedHouse = housesCache[hitTransform];

                if (pointedHouse == selectedHouse)
                    return;
                
                pointedHouse.ManageSelectedHouse(HouseAction.Highlighted);
            }
            else {
                if (pointedHouse == null)
                    return;

                if (pointedHouse == selectedHouse)
                    return;

                pointedHouse.ManageSelectedHouse(HouseAction.NotSelected);
                pointedHouse = null;
            }
        }

        public void TryToFetchHouseInfo() {
            if (pointedHouse == null)
                return;
            
            if (pointedHouse != selectedHouse) {
                if (selectedHouse != null) {
                    selectedHouse.isSelected = false;
                    selectedHouse.ManageSelectedHouse(HouseAction.NotSelected);
                }
                selectedHouse = pointedHouse;
                selectedHouse.ManageSelectedHouse(HouseAction.Selected);
            }
        }
    }
}