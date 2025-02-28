using Content.Shared.Actions;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Content.Shared.Toggleable;

namespace Content.Server._Shitmed.Cybernetics;

public sealed class ToggleProtokineticSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly IPrototypeManager _protoMan = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ToggleProtokineticComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ToggleProtokineticComponent, ToggleActionEvent>(OnActionUsed);
    }

    private void OnStartup(EntityUid uid, ToggleProtokineticComponent comp, ComponentStartup args)
    {
        _actions.AddAction(uid, ref comp.ToggleActionEntity, comp.ToggleAction);

        if (string.IsNullOrEmpty(comp.ToggleAction))
            return;
    }
    // Im sorry.
    private void OnActionUsed(EntityUid uid, ToggleProtokineticComponent comp, ToggleActionEvent args)
    {
        if (string.IsNullOrEmpty(comp.ItemPrototype) || !_protoMan.HasIndex(comp.ItemPrototype))
        {
            _popup.PopupEntity(Loc.GetString("cuffable-component-cannot-interact-message"), uid, uid);
            return;
        }

        if (!TryComp<TransformComponent>(uid, out var transform))
            return;

        // check if we have that item in the hands
        if (comp.SpawnedItem.IsValid() && _handsSystem.IsHolding(uid, comp.SpawnedItem))
        {
            _handsSystem.TryDrop(uid, comp.SpawnedItem, checkActionBlocker: false);
            EntityManager.DeleteEntity(comp.SpawnedItem);
            comp.SpawnedItem = EntityUid.Invalid;
            return;
        }

        if (!_handsSystem.TryGetEmptyHand(uid, out var emptyHand))
        {
            _popup.PopupEntity(Loc.GetString("wieldable-component-no-hands"), uid, uid);
            return;
        }

        var item = EntityManager.SpawnEntity(comp.ItemPrototype, transform.Coordinates);
        if (!_handsSystem.TryPickup(uid, item, emptyHand))
        {
            EntityManager.DeleteEntity(item);
            return;
        }

        comp.SpawnedItem = item;
    }
}
/// <summary>
/// Event that triggers when ToggleProtoKinetic
/// </summary>
public sealed partial class ToggleProtoKineticActionEvent : InstantActionEvent { }
