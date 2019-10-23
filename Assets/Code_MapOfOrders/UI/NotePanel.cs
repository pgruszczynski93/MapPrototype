using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code_MapOfOrders.UI {
    public class NotePanel : MonoBehaviour {
        [SerializeField] TextMeshProUGUI budget;
        [SerializeField] TextMeshProUGUI choices;
        [SerializeField] TextMeshProUGUI areas;

        public void LoadData(string budgetData, int choicesData, int areasData) {
            TweenUIAnimations.FadeAndUpdateText(budget, budgetData);
            TweenUIAnimations.FadeAndUpdateText(choices, choicesData.ToString());
            TweenUIAnimations.FadeAndUpdateText(areas, areasData.ToString());
        }
    }
}
