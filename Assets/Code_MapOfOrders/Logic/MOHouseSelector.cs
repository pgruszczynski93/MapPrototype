using System.Collections.Generic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOHouseSelector : MonoBehaviour {

        const string HOUSE_TAG = "TestSelection";
        
        [SerializeField] MOMapOrderHouse[] mapOfOrdersHouses;

        bool initialised;

        Camera mapOrderCamera;
        Ray selectionRay;
        MOMapOrderHouse selectedHouse;
        Dictionary<Transform, MOMapOrderHouse> housesCache;

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

        public void TryToHighlightObject(Vector3 pointerPos, bool isTryingToSelect) {
            selectionRay = mapOrderCamera.ScreenPointToRay(pointerPos);
            RaycastHit hitInfo;

            if (!Physics.Raycast(selectionRay, out hitInfo)) 
                return;
            
            if (hitInfo.collider.CompareTag(HOUSE_TAG)) {
                selectedHouse = housesCache[hitInfo.transform];
                if (isTryingToSelect) {
                    selectedHouse.ChangeHighlightMaterial(HouseAction.Selected);
                    selectedHouse.ShowSelectedHouseInfo();
                }

                else {
                    selectedHouse.ChangeHighlightMaterial(HouseAction.Highlighted);
                }
            }
            else {
                if (selectedHouse == null)
                    return;
                    
                selectedHouse.ChangeHighlightMaterial(HouseAction.Undefined);
            }
        }
        
    }
}