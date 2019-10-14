using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    [System.Serializable]
    public struct MOMouseInputData {
        public MouseAction mouseAction;
        public int scrollValue;
        public Vector2 pointerPosition;
    }
}