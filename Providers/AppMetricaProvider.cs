namespace Morpeh.Analytics.Providers {
#if MORPEH_APPMETRICA_SDK
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "AppMetricaProvider", menuName = "ScriptableObjects/Analytics/AppMetricaProvider", order = 0)]
    public class AppMetricaProvider : AnalyticsProvider {
        [SerializeField]
        private AppMetrica prefab;
        [SerializeField]
        private string androidKey;
        [SerializeField]
        private string iOSkey;

        public void Initialize(){
            if (this.prefab != null) {
                try {
                    var appMetricaPrefab = Instantiate(prefab);
                    DontDestroyOnLoad(appMetricaPrefab);

                    appMetricaPrefab.ApiKeyAndroid = androidKey;
                    appMetricaPrefab.ApiKeyIOS = iOSkey;
#if MORPEH_LUNARCONSOLE_SDK
                    AnalyticsVariables.AppMetricaAndroid.Set(androidKey);
                    AnalyticsVariables.AppMetricaiOS.Set(iOSkey);
#endif
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
        }

        public void SendEvent(AnalyticsEventComponent component) {
            if (string.IsNullOrWhiteSpace(component.revenueReceipt)) {
                AppMetrica.Instance.ReportEvent(component.eventName, (Dictionary<string, object>) component.customData);
            }
            else {
                var revenue   = new YandexAppMetricaRevenue(component.revenueValue, component.revenueCurrency);
                revenue.ProductID = component.revenueProductId;

                var yaReceipt = new YandexAppMetricaReceipt();
                
                var receipt   = JsonUtility.FromJson<Receipt>(component.revenueReceipt);
#if UNITY_ANDROID
                var payload = JsonUtility.FromJson<PayloadAndroid>(receipt.Payload);
                
                yaReceipt.Signature = payload.signature;
                yaReceipt.Data      = payload.json;
#elif UNITY_IOS
                yaReceipt.TransactionID = receipt.TransactionID;
                yaReceipt.Data          = receipt.Payload;
#endif

                revenue.Receipt = yaReceipt;
                
                AppMetrica.Instance.ReportRevenue(revenue);
            }
        }

        [Serializable]
        private struct Receipt {
            public string Store;
            public string TransactionID;
            public string Payload;
        }

        [Serializable]
        private struct PayloadAndroid {
            public string json;
            public string signature;
        }
    }
#endif
}