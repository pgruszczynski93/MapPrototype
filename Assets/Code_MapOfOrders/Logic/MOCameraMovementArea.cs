using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    [System.Serializable]
    public struct MOBorders
    {
        public float top;
        public float bottom;
        public float left;
        public float right;
    }

    public class MOCameraMovementArea : MonoBehaviour
    {
        [SerializeField] private BoxCollider leftBorder;
        [SerializeField] private BoxCollider rightBorder;
        [SerializeField] private BoxCollider topBorder;
        [SerializeField] private BoxCollider bottomBorder;

        public MOBorders MapBorders
        {
            get
            {
                var leftMapBorder = leftBorder.center.x + leftBorder.size.x * 0.5f;
                var rightMapBorder = rightBorder.center.x - rightBorder.size.x * 0.5f;
                var topMapBorder = topBorder.center.z - topBorder.size.z * 0.5f;
                var borderMapBorder = bottomBorder.center.z + bottomBorder.size.z * 0.5f;

                return new MOBorders
                {
                    top = topMapBorder,
                    bottom = borderMapBorder,
                    left = leftMapBorder,
                    right = rightMapBorder
                };
            }
        }
    }
}