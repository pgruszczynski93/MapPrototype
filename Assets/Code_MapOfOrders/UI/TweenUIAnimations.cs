using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code_MapOfOrders.UI {
    public class TweenUIAnimations {
        static float duration = 0.5f;
        static float offset = -(Screen.height / 2);
        public static void FadeAndUpdateText(TextMeshProUGUI tmp, string text) {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(tmp.DOFade(0, duration).OnComplete(() => tmp.text = text));
            mySequence.Append(tmp.DOFade(1, duration));
        }

        public static void MoveDown(RectTransform view) {
            view.DOMoveY(offset, 1);
        }
        public static void MoveUp(RectTransform view) {
            view.DOMoveY(0, 1);
        }
    }
}
