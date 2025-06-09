using Robust.Shared.GameObjects;

namespace Content.Goobstation.Common.Style
{
    public sealed class StyleRankChangedEvent : EntityEventArgs
    {
        public EntityUid Uid { get; }
        public StyleRank OldRank { get; }
        public StyleRank NewRank { get; }

        public StyleRankChangedEvent(EntityUid uid, StyleRank oldRank, StyleRank newRank)
        {
            Uid = uid;
            OldRank = oldRank;
            NewRank = newRank;
        }
    }
}
