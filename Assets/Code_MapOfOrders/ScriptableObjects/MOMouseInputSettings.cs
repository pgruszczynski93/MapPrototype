using System.ComponentModel;
using UnityEngine;

namespace HGTV.MapsOfOrders {
    
    [System.Serializable]
    public struct MOMouseInputSettings {
        [Range(0,1)] public float minViewportValueScrollAction;
        [Range(0,1)] public float maxViewportValueScrollAction;
    }
}