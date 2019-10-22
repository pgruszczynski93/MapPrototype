using System.Collections.Generic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOHouseSelector : MonoBehaviour {
        const string HOUSE_TAG = "TestSelection";

        [SerializeField] MOMapOrderHouse[] mapOfOrdersHouses;
        [SerializeField] MOMapOrderHouse highlightedHouse;
        [SerializeField] MOMapOrderHouse selectedHouse;

        bool initialised;
        Camera mapOrderCamera;
        Ray selectionRay;

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
        
        public void TryToSelectObject(Vector3 pointerPos, SelectionType selectionType) {
            selectionRay = mapOrderCamera.ScreenPointToRay(pointerPos);

            if (!Physics.Raycast(selectionRay, out var hitInfo))
                return;

            if (hitInfo.collider.CompareTag(HOUSE_TAG)) {
                var hitTransform = hitInfo.transform;

                if (!CanSetCurrentlyHighlightedObject(hitTransform) || highlightedHouse == selectedHouse)
                    return;

                SetStatusOfPointedObject(selectionType);
            }
            else
            {
                TryToResetSelection(selectionType);
            }
        }

        private void TryToResetSelection(SelectionType selectionType)
        {
            if (highlightedHouse != null && highlightedHouse != selectedHouse)
                highlightedHouse.ManageSelectedHouse(SelectionType.Undefined);

            if (selectionType != SelectionType.Selection || selectedHouse == null)
                return;

            selectedHouse.ManageSelectedHouse(SelectionType.Undefined);
            highlightedHouse = null;
            selectedHouse = null;
        }

        private void SetStatusOfPointedObject(SelectionType selectionType)
        {
            if (selectionType == SelectionType.Selection)
                TryToSetSelectedObject();
            else
                highlightedHouse.ManageSelectedHouse(SelectionType.Highlight);
        }

        private void TryToSetSelectedObject()
        {
            if (selectedHouse != null)
            {
                selectedHouse.IsSelcted = false;
                selectedHouse.ManageSelectedHouse(SelectionType.Undefined);
            }

            selectedHouse = highlightedHouse;
            selectedHouse.ManageSelectedHouse(SelectionType.Selection);
        }
        
        bool CanSetCurrentlyHighlightedObject(Transform dictKey)
        {
            if (!housesCache.TryGetValue(dictKey, out var selectedObj))
                return false;

            highlightedHouse = housesCache[dictKey];
            return true;
        }
    }
}