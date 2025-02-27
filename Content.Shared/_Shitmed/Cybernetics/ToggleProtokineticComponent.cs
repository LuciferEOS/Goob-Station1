using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Server._Shitmed.Cybernetics;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ToggleProtokineticComponent : Component
{
    [DataField(required: true), AutoNetworkedField]
    public EntProtoId ToggleAction = "ActionToggleProtoKinetic";

    [DataField, AutoNetworkedField]
    public EntityUid? ToggleActionEntity;

    [DataField, AutoNetworkedField]
    public string? ItemPrototype = "HandProtoKinetic";
}

