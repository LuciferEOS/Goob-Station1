using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Goobstation.Shared.Hustlers;

[RegisterComponent, AutoGenerateComponentState]
public sealed partial class HustleActionComponent : Component
{
    [DataField]
    public string HustleState = "hustle";

    [DataField]
    public string DefaultState = "base";

    [DataField]
    public float ActionDuration = 1.6f;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string HustleActionId = "ActionHustle";

    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;

    [ViewVariables]
    public TimeSpan? HustleEndTime;
}
