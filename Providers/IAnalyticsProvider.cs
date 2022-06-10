namespace Morpeh.Analytics.Providers {
    public interface IAnalyticsProvider {
        void Initialize();
        void SendEvent(AnalyticsEventComponent component);
    }
}