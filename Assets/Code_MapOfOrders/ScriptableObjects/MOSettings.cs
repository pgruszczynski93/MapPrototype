using UnityEngine;

namespace HGTV.MapsOfOrders {
    [CreateAssetMenu(menuName = "House Flipper/HGTV/Map of orders setups/Settings Master")]
    public class MOSettings : ScriptableObject {
        [SerializeField] MOCameraSetup cameraSetup;
        [SerializeField] MOMouseInputSetup inputSetup;
    }
}