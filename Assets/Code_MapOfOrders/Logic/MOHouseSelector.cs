using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOHouseSelector : MonoBehaviour {

        MOMapOrderHouse lastHouse;
        Ray selectionRay;
        public void Show(Ray ray) {
            selectionRay = ray;
            TryToFetchObjectData();
        }

        void TryToFetchObjectData() {
            RaycastHit hitInfo;

            if (Physics.Raycast(selectionRay, out hitInfo)) {
                if (hitInfo.collider.CompareTag("TestSelection")) {
                    hitInfo.collider.gameObject.GetComponent<MOMapOrderHouse>().ShowSelectedHouseInfo();
                    hitInfo.collider.gameObject.GetComponent<MOMapOrderHouse>().ChangeHighlightMaterial(HouseAction.Selected);
                }
            }
        }

        public void TryToHighlightObject(Ray ray) {
            RaycastHit hitInfo;

            Debug.Log("TryToHighlightObject ");
            selectionRay = ray;
            if (Physics.Raycast(selectionRay, out hitInfo)) {
                if (hitInfo.collider.CompareTag("TestSelection")) {
                    Debug.Log("Highlight");
                    lastHouse = hitInfo.collider.gameObject.GetComponent<MOMapOrderHouse>();
                    lastHouse.ChangeHighlightMaterial(HouseAction.Highlighted);
                }
                else {
                    Debug.Log("Reset");
                    if (lastHouse == null)
                        return;
                    
                    lastHouse.ChangeHighlightMaterial(HouseAction.Undefined);
                }
            }
        }
        
    }
}