using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEditor;
using UnityEngine;

namespace Code_MapOfOrders.Logic {
    public class MOMasterBehaviour : MonoBehaviour {
        void Awake() {
            Initialise();
        }

        void Initialise() {
            //todo: fix to OnMapStarted because OnMapIconSelected will be selected from laptop
            MOEvents.BroadcastOnMapStarted();
        }

        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            MOEvents.OnMapExit += TryToExitMap;
        }

        void RemoveEvents() {
            MOEvents.OnMapExit -= TryToExitMap;
        }

        void Update() {
            MOEvents.BroadcastOnUpdate();
        }

        void LateUpdate() {
            MOEvents.BroadcastOnLateUpdate();
        }

        void TryToExitMap() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}