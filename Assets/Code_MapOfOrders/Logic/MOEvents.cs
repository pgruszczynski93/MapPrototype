using System;
using Code_MapOfOrders.Logic;

namespace DefaultNamespace {
    public static class MOEvents {

        public static event Action OnMapStarted;
        public static event Action OnLaptopMapIconSelected;
        public static event Action OnUpdate;
        public static event Action<MOMouseInputData> OnMouseInputCollected;
                
        public static void BroadcastOnMapStarted() {
            OnMapStarted?.Invoke();
        }
        
        public static void BroadcastOnMapIconSelected() {
            OnLaptopMapIconSelected?.Invoke();
        }
        
        public static void BroadcastOnUpdate() {
            OnUpdate?.Invoke();
        }

        public static void BroadcastOnMouseInput(MOMouseInputData inputData) {
            OnMouseInputCollected?.Invoke(inputData);
        }
    }
}