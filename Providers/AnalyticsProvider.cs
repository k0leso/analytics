namespace Morpeh.Analytics.Providers {
    using UnityEngine.Purchasing;
    using UnityEngine.Purchasing.Security;
    using UnityEngine;
    using System.Collections.Generic;

    public abstract class AnalyticsProvider : ScriptableObject, IAnalyticsProvider {
        public abstract void Initialize();

        public abstract void Send(string name, Dictionary<string, object> args);
    }
}