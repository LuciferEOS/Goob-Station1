using Content.Shared.Damage;
using Content.Shared.Hands.Components;
using Content.Shared._Shitmed.Cybernetics;
using Content.Shared.Hands;

namespace Content.Server._Shitmed.Cybernetics;

public sealed class HandDamageModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HandDamageModifierComponent, RequestSetHandEvent>(OnHandChanged);
    }

    private void OnHandChanged(EntityUid uid, HandDamageModifierComponent component, ref RequestSetHandEvent args)
    {
        if (!TryComp(uid, out HandsComponent? hands))
            return;

        var isLeftHandActive = hands.ActiveHand?.Location == HandLocation.Left;
        var shouldApply = component.ApplyToRightHand != isLeftHandActive;

        if (shouldApply)
        {
            if (TryComp<DamageableComponent>(uid, out var damageable))
            {
                damageable.Damage += component.DamageBonus;
                Dirty(uid, damageable);
            }
        }
        else
        {
            if (TryComp<DamageableComponent>(uid, out var damageable))
            {
                damageable.Damage -= component.DamageBonus;
                Dirty(uid, damageable);
            }
        }
        if (shouldApply)
        {
            var damageMod = EnsureComp<HandDamageModifierComponent>(uid);
            damageMod.Multiplier -= component.DamageBonus;
            Dirty(uid, damageMod);
        }
    }
}
