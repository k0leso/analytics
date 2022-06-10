namespace Morpeh.Analytics.Providers {
#if MORPEH_GAMEANALYTICS_SDK
    using System;
    using GameAnalyticsSDK;
    using UnityEngine;

    [CreateAssetMenu(fileName = "GameAnalyticsProvider", menuName = "ScriptableObjects/Analytics/GameAnalyticsProvider", order = 0)]
    public class GameAnalyticsProvider : IAnalyticsProvider {
        [SerializeField]
        private GameObject prefab;
        public void Initialize(){
            if (prefab != null) {
                try {
                    var gameAnalyticsPrefab = Instantiate(prefab);
                    DontDestroyOnLoad(gameAnalyticsPrefab);

                    GameAnalytics.Initialize();
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }

        public void SendEvent(AnalyticsEventComponent component) {
            if (string.IsNullOrWhiteSpace(component.revenueReceipt)) {
                GameAnalytics.NewDesignEvent(this.ToGaFormat(component));
            }
            else {
                var receipt = JsonUtility.FromJson<Receipt>(component.revenueReceipt);
#if UNITY_ANDROID
                var payload = JsonUtility.FromJson<PayloadAndroid>(receipt.Payload);
                
                GameAnalytics.NewBusinessEventGooglePlay(component.revenueCurrency, (int) component.revenueValue, "", component.revenueProductId, "", component.revenueReceipt ,payload.signature);
#endif
#if UNITY_IOS
                GameAnalytics.NewBusinessEvent(component.revenueCurrency, (int) component.revenueValue, component.revenueProductId, component.revenueProductId, component.revenueReceipt);
#endif
            }
        }

        private string ToGaFormat(AnalyticsEventComponent component) {
            var data = $"{component.eventName}:";
            var str = component.ToString();
            str = str.Replace(">", ":").Replace("=", ":");
            data += str;
            return data;
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
