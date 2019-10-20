using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

namespace Code_MapOfOrders.Logic
{
    public class MOMasterBehaviour : MonoBehaviour
    {
        void Awake()
        {
            Initialise();
        }

        void Initialise()
        {
            //todo: fix to OnMapStarted because OnMapIconSelected will be selected from laptop
            MOEvents.BroadcastOnMapStarted();
        }

        void Update()
        {
            MOEvents.BroadcastOnUpdate();
        }

        void LateUpdate()
        {
            MOEvents.BroadcastOnLateUpdate();
        }
    }
}