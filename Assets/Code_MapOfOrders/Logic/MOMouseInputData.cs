using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    [System.Serializable]
    public struct MOMouseInputData {
        public bool isOnTheScreenEdge;
        public MouseAction mouseAction;
        public float scrollValue;
        public Vector2 pointerPosition;
    }
}