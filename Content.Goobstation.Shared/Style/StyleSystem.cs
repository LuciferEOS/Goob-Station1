using Content.Goobstation.Common.Style;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Goobstation.Shared.Style
{
    public sealed class StyleSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly IPrototypeManager _proto = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<StyleCounterComponent, MapInitEvent>(OnMapInit);
            SubscribeLocalEvent<StyleCounterComponent, StyleEventMessage>(OnStyleEvent);
        }

        private void OnMapInit(EntityUid uid, StyleCounterComponent component, MapInitEvent args)
        {
            // UpdateRank(uid, component);
        }
        private void OnStyleEvent(EntityUid uid, StyleCounterComponent component, StyleEventMessage args)
        {
            AddStyleEvent(uid, args.EventId, component);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            var query = EntityQueryEnumerator<StyleCounterComponent>();
            while (query.MoveNext(out var uid, out var style))
            {
                // Apply decay with multiplier
                style.CurrentPoints = Math.Max(0, style.CurrentPoints -
                    (style.BaseDecayPerSecond * style.CurrentMultiplier * frameTime));

                // Remove old events
                if (_timing.CurTime - style.LastEventTime > TimeSpan.FromSeconds(10))
                {
                    style.RecentEvents.Clear();
                }

                // UpdateRank(uid, style);
            }
        }

        public void AddStyleEvent(EntityUid uid, string eventId, StyleCounterComponent? component = null)
        {
            if (!Resolve(uid, ref component))
                return;

            if (!_proto.TryIndex<StyleEventPrototype>(eventId, out var proto))
                return;

            component.CurrentPoints += proto.Points;
            component.LastEventTime = _timing.CurTime;

            // Add event to history
            var sign = proto.Points >= 0 ? "+" : "";
            var eventText = $"{sign}{proto.Points} {proto.Description}";
            component.RecentEvents.Add(eventText);

            // Keep only last 5 events
            if (component.RecentEvents.Count > 5)
                component.RecentEvents.RemoveAt(0);

            // UpdateRank(uid, component);
        }

        private void UpdateRank(EntityUid uid, StyleCounterComponent style)
        {
            var newRank = style.CurrentPoints switch
            {
                >= 1500 => StyleRank.R,
                >= 1000 => StyleRank.SSS,
                >= 850 => StyleRank.SS,
                >= 700 => StyleRank.S,
                >= 500 => StyleRank.A,
                >= 400 => StyleRank.B,
                >= 300 => StyleRank.C,
                >= 200 => StyleRank.D,
                _ => StyleRank.F
            };

            if (newRank != style.Rank)
            {
                var oldRank = style.Rank;
                style.Rank = newRank;
                style.CurrentMultiplier = GetRankMultiplier(newRank);

                // RaiseLocalEvent(uid, new StyleRankChangedEvent(uid, oldRank, newRank));
            }
        }

        private float GetRankMultiplier(StyleRank rank)
        {
            return rank switch
            {
                StyleRank.R => 8.0f,
                StyleRank.SSS => 6.0f,
                StyleRank.SS => 4.0f,
                StyleRank.S => 3.0f,
                StyleRank.A => 2.0f,
                StyleRank.B => 1.5f,
                StyleRank.C => 1.25f,
                StyleRank.D => 1.0f,
                StyleRank.F => 0.5f,
                _ => 1.0f
            };
        }
        [Serializable, NetSerializable]
        public sealed class StyleEventMessage : EntityEventArgs
        {
            public string EventId { get; }

            public StyleEventMessage(string eventId)
            {
                EventId = eventId;
            }
        }
    }
}
