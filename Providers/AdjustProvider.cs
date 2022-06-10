namespace Morpeh.Analytics.Providers {
#if MORPEH_ADJUST_SDK
    using com.adjust.sdk;

    [CreateAssetMenu(fileName = "AdjustProvider", menuName = "ScriptableObjects/Analytics/AdjustProvider", order = 0)]
    public class AdjustProvider : AnalyticsProvider {
        [SerializeField]
        private Adjust prefab;
        [SerializeField]
        private string appToken;

        public void Initialize(){
            if (this.adjustPrefab != null) {
                try {
                    var adjustPrefab = Instantiate(prefab);
                    DontDestroyOnLoad(adjustPrefab);

                    adjustPrefab.appToken = appToken;
#if MORPEH_LUNARCONSOLE_SDK
                    AnalyticsVariables.Adjust.Set(appToken);
#endif
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }

        public void SendEvent(AnalyticsEventComponent component) {
            var @event = new AdjustEvent(component.eventName);

            if (string.IsNullOrWhiteSpace(component.revenueTransactionId) == false) {
                @event.setRevenue(component.revenueValue, component.revenueCurrency);
                @event.setTransactionId(component.revenueTransactionId);
            }

            if (component.customData != null && component.customData.Count != 0) {
                foreach (var customDataPair in component.customData) {
                    @event.addCallbackParameter(customDataPair.Key, customDataPair.Value.ToString());
                }
            }
            
            Adjust.trackEvent(@event);
        }
    }
#endif
}