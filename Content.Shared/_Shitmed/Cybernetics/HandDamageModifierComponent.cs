using Robust.Shared.GameStates;
using Content.Shared.Damage;

namespace Content.Shared._Shitmed.Cybernetics;

[RegisterComponent, NetworkedComponent]
public sealed partial class HandDamageModifierComponent : Component
{
    /// <summary>
    ///     Applies more damage to the left hand if set on false, and to the right hand if set on true
    /// </summary>
    [DataField("applyToRightHand"), ViewVariables(VVAccess.ReadWrite)]
    public bool ApplyToRightHand = false;

    [DataField("damageBonus", required: true)]
    public DamageSpecifier DamageBonus = new();
}
