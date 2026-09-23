using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Imperial.Medieval.Myrmex
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class MyrmexHungerComponent : Component
    {
        [DataField, AutoNetworkedField]
        public TimeSpan? LastEaten;

        [DataField, AutoNetworkedField]
        public float EatCooldownSeconds = 90;

        [DataField, AutoNetworkedField]
        public float SecondsToHungry = 1000;

        // imperial medieval - tracks whether the myrmex was hungry last check, so speed is only
        // recalculated when the state actually flips (avoids per-tick refresh that desyncs movement)
        [DataField]
        public bool WasHungry;

        // imperial medieval - server time of the next hunger check (throttled, not every tick)
        [DataField]
        public TimeSpan NextHungerCheck;

        [DataField, AutoNetworkedField]
        public float HungrySpeedModifier = 0.5f;

        // imperial medieval - speed multiplier while dragging another myrmex
        [DataField, AutoNetworkedField]
        public float HiveDragSpeedModifier = 1.15f;

        // imperial medieval - caste-specific extra resist to incoming stamina damage, on top of
        // whatever food buffs give. 1 = no effect (default for every caste except Queen).
        [DataField, AutoNetworkedField]
        public float CasteStaminaDamageResist = 1f;

        [DataField, AutoNetworkedField]
        public List<MyrmexBuff> Buffs = [];

        [DataField, AutoNetworkedField]
        public EntProtoId IconPrototype = "HungerIconStarving";
    }
}
