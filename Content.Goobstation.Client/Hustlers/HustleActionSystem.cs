using Content.Goobstation.Shared.Hustlers;
using Content.Shared.Actions;
using Content.Shared.Interaction.Components;
using Robust.Client.GameObjects;
using Robust.Shared.Timing;

namespace Content.Goobstation.Client.Hustlers;

public sealed class HustlerActionsSystem : EntitySystem
{
    [Dependency] private readonly SpriteSystem _sprite = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HustleActionComponent, HustleActionEvent>(OnHustleAction);
        // SubscribeLocalEvent<HustleActionComponent, ComponentShutdown>(OnComponentShutdown);
    }
    // private void OnComponentShutdown(EntityUid uid, HustleActionComponent component, ComponentShutdown args)
    // {
    //     if (TryComp<SpriteComponent>(uid, out var sprite))
    //         _sprite.LayerSetRsiState((uid, sprite), 0, component.DefaultState);
    // }

    private void OnHustleAction(EntityUid uid, HustleActionComponent component, HustleActionEvent args)
    {
        if (HasComp<BlockMovementComponent>(uid)
            || component.HustleEndTime != null)
            return;

        StartHustle(uid, component);
    }
    private void StartHustle(EntityUid uid, HustleActionComponent component)
    {
        EnsureComp<BlockMovementComponent>(uid);
        component.HustleEndTime = _gameTiming.CurTime + TimeSpan.FromSeconds(component.ActionDuration);
    }
}
