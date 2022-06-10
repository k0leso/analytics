namespace Morpeh.Analytics {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public struct AnalyticsEventComponent : IComponent {
        public string providerName;
        public string eventName;

        public string revenueProductId;
        public double revenueValue;
        public string revenueCurrency;
        public string revenueTransactionId;
        public string revenueReceipt;

        public Dictionary<string, object> customData;

        private static void AppendDictionary(StringBuilder builder, Dictionary<string, object> dictionary) {
            foreach (var dataPair in dictionary) {
                if (dataPair.Value is Dictionary<string, object> value) {
                    builder.Append($">");
                    AppendDictionary(builder, value);

                    continue;
                }

                builder.Append($"{dataPair.Key}={dataPair.Value}{Environment.NewLine}");
            }
        }

        public override string ToString() {
            var customDataStringBuilder = new StringBuilder();

            if (this.customData != null) {
                AppendDictionary(customDataStringBuilder, this.customData);
            }

            var revenueString = string.Empty;

            if (string.IsNullOrWhiteSpace(this.revenueTransactionId) == false) {
                revenueString = $"Id:{this.revenueProductId} Price:{this.revenueValue} Currency:{this.revenueCurrency} TransactionId:{this.revenueTransactionId}";
            }

            var result = $"{this.eventName}";

            if (this.customData != null) {
                result += $" {customDataStringBuilder}";
            }

            if (string.IsNullOrWhiteSpace(revenueString) == false) {
                result += $" {revenueString}";
            }

            return result;
        }
    }
}