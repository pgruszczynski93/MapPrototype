using DefaultNamespace;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOMasterBehaviour : MonoBehaviour{

        void Start() {
            Initialise();
        }
        
        void Initialise() {
            //todo: fix to OnMapStarted because OnMapIconSelected will be selected from laptop
            MOEvents.BroadcastOnMapIconSelected();
        }

        void Update() {
            MOEvents.BroadcastOnUpdate();
        }

        void LateUpdate() {
            MOEvents.BroadcastOnLateUpdate();
        }
    }
}