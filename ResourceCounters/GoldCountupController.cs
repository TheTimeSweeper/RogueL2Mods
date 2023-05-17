using System;
using TMPro;
using UnityEngine;

namespace ResourceCountersMod {
    public class GoldCountupController : ResourceCountupController {

        private RectTransform goldModTextTransform;

        public void init(TMP_Text resourceText, ResourceReporter resourceReporter, RectTransform goldModTextTransform) {
            base.init(resourceText, resourceReporter);
            this.goldModTextTransform = goldModTextTransform;
        }

        protected override void UpdateTextAmounts() {
            base.UpdateTextAmounts();

            Vector2 anchoredPosition2 = goldModTextTransform.anchoredPosition;
            anchoredPosition2.x = addedResourceText.rectTransform.anchoredPosition.x + addedResourceText.rectTransform.sizeDelta.x + 50f;
            goldModTextTransform.anchoredPosition = anchoredPosition2;
        }
    }
}
