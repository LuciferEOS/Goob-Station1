using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Common.Style
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class StyleCounterComponent : Component
    {
        [ViewVariables, AutoNetworkedField]
        public StyleRank Rank = StyleRank.D;

        [ViewVariables, AutoNetworkedField]
        public float CurrentPoints = 0;

        [DataField("baseDecayRate"), ViewVariables, AutoNetworkedField]
        public float BaseDecayPerSecond = 1f;

        [ViewVariables, AutoNetworkedField]
        public float CurrentMultiplier = 1f;

        [ViewVariables, AutoNetworkedField]
        public List<string> RecentEvents = new();

        [ViewVariables, AutoNetworkedField]
        public TimeSpan LastEventTime;
    }

    [Serializable, NetSerializable]
    public enum StyleRank
    {
        R,
        SSS,
        SS,
        S,
        A,
        B,
        C,
        D,
        F
    }
}
