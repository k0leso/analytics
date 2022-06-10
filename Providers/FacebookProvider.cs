namespace Morpeh.Analytics.Providers {
#if MORPEH_FACEBOOK_SDK
    using System.Collections.Generic;
    using Facebook.Unity;

    [CreateAssetMenu(fileName = "FacebookProvider", menuName = "ScriptableObjects/Analytics/FacebookProvider", order = 0)]
    public class FacebookProvider : IAnalyticsProvider {
        public void Initialize(){
            try {
#if MORPEH_LUNARCONSOLE_SDK
                AnalyticsVariables.FacebookAppId.Set(Facebook.Unity.Settings.FacebookSettings.AppId);
#endif
                FB.Init();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }

        public void SendEvent(AnalyticsEventComponent component) {
            if (FB.IsInitialized == false) {
                return;
            }

            if (string.IsNullOrWhiteSpace(component.revenueTransactionId)) {
                FB.LogAppEvent(component.eventName, null, component.customData);
            }
            else {
                FB.LogPurchase((decimal) component.revenueValue, component.revenueCurrency, new Dictionary<string, object> {
                    {"ProductId", component.revenueProductId}
                });
            }
        }
    }
#endif
}