using System.Collections.Generic;
using Code_MapOfOrders.UI;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOHouseSelector : MonoBehaviour
    {
        const string HOUSE_TAG = "TestSelection";

        [SerializeField] MOMapOrderHouse[] mapOfOrdersHouses;
        [SerializeField] MOMapOrderHouse highlightedHouse;
        [SerializeField] MOMapOrderHouse selectedHouse;
        [SerializeField] MapView mapView;

        bool initialised;
        Camera mapOrderCamera;
        Ray selectionRay;

        Dictionary<Transform, MOMapOrderHouse> housesCache;

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            TryToInitialiseHousesCache();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            MOEvents.OnSelect += TryToSelectObject;
        }
        void RemoveEvents()
        {
            MOEvents.OnSelect -= TryToSelectObject;
        }

        void TryToInitialiseHousesCache()
        {
            if (mapOfOrdersHouses.Length == 0 || mapOfOrdersHouses == null)
            {
                Debug.LogError("No selectable houses assigned.");
                return;
            }

            housesCache = new Dictionary<Transform, MOMapOrderHouse>();
            for (var i = 0; i < mapOfOrdersHouses.Length; i++)
            {
                var currentHouse = mapOfOrdersHouses[i];
                housesCache.Add(currentHouse.transform, currentHouse);
            }
        }

        void Start()
        {
            Initialise();
        }

        public void SetupHouseSelector(Camera cam)
        {
            mapOrderCamera = cam;
        }

        void TryToSelectObject(Vector3 pointerPos, MapSelectionType mapSelectionType)
        {
            selectionRay = mapOrderCamera.ScreenPointToRay(pointerPos);

            if (!Physics.Raycast(selectionRay, out var hitInfo))
                return;

            if (hitInfo.collider.CompareTag(HOUSE_TAG))
            {
                var hitTransform = hitInfo.transform;

                if (!CanSetCurrentlyHighlightedObject(hitTransform) || highlightedHouse == selectedHouse)
                    return;

                SetStatusOfPointedObject(mapSelectionType);
            }
            else
            {
                TryToResetSelection(mapSelectionType);
            }
        }

        private void TryToResetSelection(MapSelectionType mapSelectionType)
        {
            if (highlightedHouse != null && highlightedHouse != selectedHouse)
                highlightedHouse.ManageSelectedHouse(MapSelectionType.Undefined);

            if (mapSelectionType != MapSelectionType.Selection || selectedHouse == null)
                return;

            selectedHouse.ManageSelectedHouse(MapSelectionType.Undefined);
            highlightedHouse = null;
            selectedHouse = null;
        }

        private void SetStatusOfPointedObject(MapSelectionType mapSelectionType)
        {
            if (mapSelectionType == MapSelectionType.Selection)
                TryToSetSelectedObject();
            else
                highlightedHouse.ManageSelectedHouse(MapSelectionType.Highlight);
        }

        private void TryToSetSelectedObject()
        {
            if (selectedHouse != null)
            {
                selectedHouse.IsSelcted = false;
                selectedHouse.ManageSelectedHouse(MapSelectionType.Undefined);
            }

            selectedHouse = highlightedHouse;
            DisplayData();
            selectedHouse.ManageSelectedHouse(MapSelectionType.Selection);
        }

        private void DisplayData()
        {
            mapView.LoadData(selectedHouse.GetHouseInfo());
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