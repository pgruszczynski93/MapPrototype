using System.ComponentModel;
using UnityEngine;

namespace HGTV.MapsOfOrders {
    
    [System.Serializable]
    public struct MOMouseInputSettings {
        [Range(1f, 10f)] public int edgeThickness;
    }
}