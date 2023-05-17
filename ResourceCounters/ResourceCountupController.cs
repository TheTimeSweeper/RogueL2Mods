using System;
using System.Security;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceCountersMod {

    public class ResourceCountupController : MonoBehaviour{

        const float TIMER_DELAY = 1.2f;
        const float COUNTING_TIME = 0.4f;

        protected TMP_Text baseResourceText;
        protected TMP_Text addedResourceText;

        private RectTransform rectTransform;

        private int _lastResourceAmount = -1;

        private int _currentAddedAmount;
        private int _totalAddedAmount;

        private float _tim = 10;

        ResourceReporter _resourceChangedReporter;

        public virtual void init(TMP_Text resourceText, ResourceReporter resourceReporter) {

            initResourceText(resourceText);

            _resourceChangedReporter = resourceReporter;

            _resourceChangedReporter.onPostResourceChanged += ResourceCountersMod_onPostResourceChanged;
        }

        void OnDestroy() {

            _resourceChangedReporter.onPostResourceChanged -= ResourceCountersMod_onPostResourceChanged;
        }

        protected void initResourceText(TMP_Text resourceText) {
            baseResourceText = resourceText;
            addedResourceText = Instantiate(baseResourceText, baseResourceText.transform.parent);
            addedResourceText.transform.SetSiblingIndex(addedResourceText.transform.GetSiblingIndex() - 1);
            addedResourceText.text = string.Empty;

            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void ResourceCountersMod_onPostResourceChanged(int amount) {

            if (_lastResourceAmount == -1) {
                _lastResourceAmount = amount;
            }

            int difference = amount - _lastResourceAmount;
            _lastResourceAmount = amount;
            if (difference <= 0)
                return;

            if (_tim > TIMER_DELAY) {
                _currentAddedAmount = 0;
            }

            _currentAddedAmount += difference;
            _totalAddedAmount = _currentAddedAmount;
            _tim = 0;

            UpdateTextAmounts();
        }

        void Update () {

            _tim += Time.deltaTime;
            
            if (_tim > TIMER_DELAY + COUNTING_TIME + 1)
                return;

            if (_tim > TIMER_DELAY) {
                _currentAddedAmount = (int)UnityEngine.Mathf.Lerp(_totalAddedAmount, 0, Mathf.InverseLerp(TIMER_DELAY, TIMER_DELAY + COUNTING_TIME, _tim));
            }

            UpdateTextAmounts();
        }

        protected virtual void UpdateTextAmounts() {

            float fakeResourceAmount = _lastResourceAmount - _currentAddedAmount;

            baseResourceText.text = fakeResourceAmount.ToString();

            if (_currentAddedAmount > 0) {
                addedResourceText.text = $"<color=yellow>+{_currentAddedAmount.ToString()}";
            } else {
                addedResourceText.text = string.Empty;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
