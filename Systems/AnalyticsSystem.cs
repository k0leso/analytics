namespace Morpeh.Analytics {
    using System.Collections.Generic;
    using Providers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Analytics/Systems/" + nameof(AnalyticsSystem))]
    public class AnalyticsSystem : LateUpdateSystem {
        [SerializeField]
        private AnalyticsProvider[] initializeProviders;
        private Dictionary<string, IAnalyticsProvider> providers;
        
        private Filter events;

        public override void OnAwake() {
            this.events    = this.World.Filter.With<AnalyticsEventComponent>();
            this.providers = new Dictionary<string, IAnalyticsProvider>();

            foreach (var provider in initializeProviders) {
                provider.Initialize(); 
                providers.Add(provider.name, provider);
            }

        public override void OnUpdate(float deltaTime) {
            foreach (var eventEntity in this.events) {
                ref var component = ref eventEntity.GetComponent<AnalyticsEventComponent>();

                if (string.IsNullOrWhiteSpace(component.providerName) == false) {
                    var providerNames = component.providerName.Split(';');

                    foreach (var providerName in providerNames) {
                        if (this.providers.TryGetValue(providerName, out var provider) == false) {
                            continue;
                        }

                        provider.SendEvent(component);

                        Debug.Log($"[AnalyticsSystem] Provider: {providerName} {component}");
                    }
                }
                else {
                    foreach (var providerKeyPair in this.providers) {
                        var provider = providerKeyPair.Value;
                        provider.SendEvent(component);

                        Debug.Log($"[AnalyticsSystem] Provider: {providerKeyPair.Key} {component}");
                    }
                }

                this.World.RemoveEntity(eventEntity);
            }
        }
    }
}