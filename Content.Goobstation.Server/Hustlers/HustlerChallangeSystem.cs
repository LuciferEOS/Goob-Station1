using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Goobstation.Server.Hustlers;

// no its not just copied gang handshake system trust.
public sealed class HustlerChallengeSystem : EntitySystem
{
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HustlerComponent, GetVerbsEvent<InnateVerb>>(OnGetVerbs);
        SubscribeLocalEvent<HustlerChallengePendingComponent, GetVerbsEvent<InnateVerb>>(OnGetVerbsPending);
    }

    private void OnGetVerbs(EntityUid uid, HustlerComponent comp, GetVerbsEvent<InnateVerb> args)
    {
        if (!args.CanAccess
            || !args.CanInteract
            || args.Target == args.User)
            return;

        if (HasComp<HustlerChallengePendingComponent>(args.Target))
            return;

        InnateVerb duelVerb = new()
        {
            Act = () => OfferChallenge(args.User, args.Target),
            Text = Loc.GetString("hustler-challenge-verb", ("target", args.Target)),
            Icon = new SpriteSpecifier.Rsi(new("_Goobstation/Mobs/Hustlers/ninja.rsi"), "base"),
            Priority = 1
        };
        args.Verbs.Add(duelVerb);
    }

    private void OfferChallenge(EntityUid challenger, EntityUid target)
    {
        var pending = AddComp<HustlerChallengePendingComponent>(target);
        pending.Challenger = challenger;
        pending.ExpiryTime = _timing.CurTime + TimeSpan.FromSeconds(15);

        _popup.PopupEntity(Loc.GetString("hustler-challenge-offer", ("user", challenger)), target, target);
        _popup.PopupEntity(Loc.GetString("hustler-challenge-offer-self", ("target", target)), challenger, challenger);
    }

    private void OnGetVerbsPending(EntityUid uid, HustlerChallengePendingComponent comp, GetVerbsEvent<InnateVerb> args)
    {
        if (!args.CanAccess
            || !args.CanInteract
            || args.Target != comp.Challenger)
            return;

        if (_mobState.IsIncapacitated(uid))
            return;

        InnateVerb acceptVerb = new()
        {
            Act = () => AcceptChallenge(uid, comp.Challenger),
            Text = Loc.GetString("hustler-challenge-accept-verb", ("user", comp.Challenger)),
            Icon = new SpriteSpecifier.Rsi(new("_Goobstation/Mobs/Hustlers/ninja.rsi"), "base"), // todo
            Priority = 1
        };
        args.Verbs.Add(acceptVerb);
    }

    private void AcceptChallenge(EntityUid target, EntityUid challenger)
    {
        if (!Exists(challenger))
        {
            _popup.PopupEntity(Loc.GetString("hustler-challenge-invalid"), target, target);
            RemComp<HustlerChallengePendingComponent>(target);
            return;
        }

        _popup.PopupEntity(Loc.GetString("hustler-challenge-accepted", ("target", target)), challenger, challenger);
        _popup.PopupEntity(Loc.GetString("hustler-challenge-accepted-self", ("challenger", challenger)), target, target);

        RemComp<HustlerChallengePendingComponent>(target); // todo start hustle
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var now = _timing.CurTime;
        var query = EntityQueryEnumerator<HustlerChallengePendingComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.ExpiryTime > now)
                continue;

            RemCompDeferred<HustlerChallengePendingComponent>(uid);
            _popup.PopupEntity(Loc.GetString("hustler-challenge-expired"), uid, uid);

            if (Exists(comp.Challenger))
                _popup.PopupEntity(Loc.GetString("hustler-challenge-expired-other", ("target", uid)), comp.Challenger, comp.Challenger);
        }
    }
}
