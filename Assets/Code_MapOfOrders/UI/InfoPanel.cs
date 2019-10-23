using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code_MapOfOrders.UI {
    public class InfoPanel : MonoBehaviour {
        [SerializeField] TextMeshProUGUI header;
        [SerializeField] TextMeshProUGUI mainInfo;

        public void LoadData(string title, string info) {
            TweenUIAnimations.FadeAndUpdateText(header, title);
            TweenUIAnimations.FadeAndUpdateText(mainInfo, info);
        }
    }
}