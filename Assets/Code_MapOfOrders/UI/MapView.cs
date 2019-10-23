using HGTV.MapsOfOrders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code_MapOfOrders.UI {
    public class MapView : MonoBehaviour {
        [SerializeField] InfoPanel infoPanel;
        [SerializeField] NotePanel notePanel;
        [SerializeField] Button playButton;

        public void LoadData(MOOrderHouseInfo mOMapOrderInfo) {
            infoPanel.LoadData(mOMapOrderInfo.title, mOMapOrderInfo.details);
            notePanel.LoadData(mOMapOrderInfo.budget, mOMapOrderInfo.choices, mOMapOrderInfo.areas);
            playButton.onClick.AddListener(delegate { Play(mOMapOrderInfo.title); });
        }

        void Play(string title) {
            Debug.Log("PLAYING " + title);
        }
    }
}
