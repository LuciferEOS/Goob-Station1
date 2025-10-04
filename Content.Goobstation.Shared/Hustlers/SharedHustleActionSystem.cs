using Content.Shared.Actions;
using Content.Shared.Interaction.Components;
using Robust.Shared.Timing;

namespace Content.Goobstation.Shared.Hustlers;

public sealed class HustlerActionsSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly ActionContainerSystem _actcon = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HustleActionComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<HustleActionComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<HustleActionComponent, ComponentShutdown>(OnComponentShutdown);
    }
    private void OnMapInit(EntityUid uid, HustleActionComponent component, MapInitEvent args)
    {
        _actions.AddAction(uid, ref component.ActionEntity, component.HustleActionId);
    }

    private void OnComponentStartup(EntityUid uid, HustleActionComponent component, ComponentStartup args)
    {
        if (component.ActionEntity == null)
        {
            _actions.AddAction(uid, ref component.ActionEntity, component.HustleActionId);
        }
    }

    private void OnComponentShutdown(EntityUid uid, HustleActionComponent component, ComponentShutdown args)
    {

        if (component.ActionEntity != null)
            _actions.RemoveAction(uid,component.ActionEntity.Value);

    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var currentTime = _gameTiming.CurTime;
        var query = EntityQueryEnumerator<HustleActionComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.HustleEndTime == null
                || currentTime < component.HustleEndTime)
                continue;

            EndHustle(uid, component);
        }
    }

    private void EndHustle(EntityUid uid, HustleActionComponent component)
    {
        RemComp<BlockMovementComponent>(uid);
        component.HustleEndTime = null;
    }

    public void ForceEndHustle(EntityUid uid)
    {
        if (TryComp<HustleActionComponent>(uid, out var component))
            EndHustle(uid, component);
    }
}
