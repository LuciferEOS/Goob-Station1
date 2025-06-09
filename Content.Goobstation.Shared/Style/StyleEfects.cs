using Content.Goobstation.Common.Style;
using Content.Shared.Abilities;

namespace Content.Goobstation.Shared.Style
{
    public sealed class StyleEffectsSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<StyleRankChangedEvent>(OnRankChanged);
        }

        private void OnRankChanged(StyleRankChangedEvent args)
        {
            var uid = args.Uid;
            if (args.NewRank == StyleRank.R)
                EnsureComp<DogVisionComponent>(uid);

            else if (args.OldRank == StyleRank.R)
                RemCompDeferred<DogVisionComponent>(uid);
        }
    }
}
