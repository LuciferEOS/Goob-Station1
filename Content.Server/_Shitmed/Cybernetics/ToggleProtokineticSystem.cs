using Content.Shared.Actions;
using Content.Shared.Hands.EntitySystems;
using Content.Shared._Shitmed.Cybernetics;
using Content.Shared.Popups;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Server._Shitmed.Cybernetics;

public sealed class ToggleProtokineticSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ToggleProtokineticComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ToggleProtokineticComponent, ToggleProtoKineticActionEvent>(OnActionUsed);
    }

    private void OnStartup(EntityUid uid, ToggleProtokineticComponent comp, ComponentStartup args)
    {
        if (string.IsNullOrEmpty(comp.ToggleAction))
            return;

        _actions.AddAction(uid, ref comp.ToggleActionEntity, comp.ToggleAction);
    }

    private void OnActionUsed(EntityUid uid, ToggleProtokineticComponent comp, ToggleProtoKineticActionEvent args)
    {
        if (string.IsNullOrEmpty(comp.ItemPrototype))
        {
            _popup.PopupEntity(Loc.GetString("mechanism-no-item"), uid, uid);
            return;
        }
        if (!TryComp<TransformComponent>(uid, out var transform))
            return;

        if (!_handsSystem.TryGetEmptyHand(uid, out var emptyHand))
        {
            _popup.PopupEntity(Loc.GetString("mechanism-no-hand"), uid, uid);
            return;
        }
        var item = EntityManager.SpawnEntity(comp.ItemPrototype, transform.Coordinates);
        _handsSystem.TryPickup(uid, item, emptyHand);
    }
}
/// <summary>
/// Event that triggers when ToggleProtoKinetic
/// </summary>
public sealed partial class ToggleProtoKineticActionEvent : InstantActionEvent { }
