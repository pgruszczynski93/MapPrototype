using System;
using Code_MapOfOrders.Logic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class MOEvents
    {
        public static event Action OnMapStarted;
        public static event Action OnUpdate;
        public static event Action OnLateUpdate;
        public static event Action<MOMouseInputData> OnMouseInputCollected;
        public static event Action<Vector3> OnHighlight;

        public static event Action<Vector3> OnPointerPositionUpdate;
        public static event Action<Vector3> OnSelect;
        public static event Action<Vector3> OnDrag;
        public static event Action<Vector3> OnScroll;
        public static event Action<int> OnZoom;

        public static void BroadcastOnPointerPositionUpdate(Vector3 pointerPos)
        {
            OnPointerPositionUpdate?.Invoke(pointerPos);
        }

        public static void BroadcastOnHighlight(Vector3 pointerPos)
        {
            OnHighlight?.Invoke(pointerPos);
        }

        public static void BroadcastOnSelect(Vector3 pointerPos)
        {
            OnSelect?.Invoke(pointerPos);
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

        public static void BroadcastOnMouseInput(MOMouseInputData inputData)
        {
            OnMouseInputCollected?.Invoke(inputData);
        }
    }
}