using UnityEngine;

namespace HGTV.MapsOfOrders {
    [CreateAssetMenu(menuName = "House Flipper/HGTV/Map of orders setups/Create Order House")]
    public class MOOrderHouseInfo : ScriptableObject {
        public string budget;
        public int choices;
        public int areas;
        public string title;
        public string details;
    }
}