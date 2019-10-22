using System;
using Code_MapOfOrders.Logic;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace DefaultNamespace
{
    public static class MOEvents
    {
        public static event Action OnMapStarted;
        public static event Action OnUpdate;
        public static event Action OnLateUpdate;
        public static event Action<Vector3, MapSelectionType> OnSelect;
        public static event Action<Vector3> OnDrag;
        public static event Action<Vector3> OnScroll;
        public static event Action<int> OnZoom;

        public static void BroadcastOnSelect(Vector3 pointerPos, MapSelectionType selection)
        {
            OnSelect?.Invoke(pointerPos, selection);
        }

        public static void BroadcastOnDrag(Vector3 pointerPos)
        {
            OnDrag?.Invoke(pointerPos);
        }

        public static void BroadcastOnScroll(Vector3 pointerPos)
        {
            OnScroll?.Invoke(pointerPos);
        }

        public static void BroadcastOnZoom(int scrollValue)
        {
            OnZoom?.Invoke(scrollValue);
        }

        public static void BroadcastOnMapStarted()
        {
            OnMapStarted?.Invoke();
        }

        public static void BroadcastOnUpdate()
        {
            OnUpdate?.Invoke();
        }

        public static void BroadcastOnLateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}