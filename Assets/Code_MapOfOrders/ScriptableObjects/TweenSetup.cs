using DG.Tweening;
using UnityEngine;

namespace HGTV.MapsOfOrders
{
    [CreateAssetMenu(menuName = "House Flipper/HGTV/Map of orders setups/Tween setup")]
    public class TweenSetup : ScriptableObject
    {
        public TweenSettings tweenSettings;
    }

    [System.Serializable]
    public struct TweenSettings
    {
        public Ease easeType;
        [Range(0f, 10f)] public float tweenTime;
        [Range(0f, 500f)] public float positionDeltaMultiplier;
    }
}