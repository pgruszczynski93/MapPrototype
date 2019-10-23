using System.ComponentModel;
using UnityEngine;

namespace HGTV.MapsOfOrders {
    
    [System.Serializable]
    public struct MOMouseInputSettings {
        [Range(0f, 10f)] public float horizontalEdgeThicknessPercentage;
        [Range(0f, 10f)] public float verticalEdgeThicknessPercentage;
    }
}