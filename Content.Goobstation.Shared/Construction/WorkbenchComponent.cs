using Content.Shared.Construction.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Goobstation.Shared.Construction;

[RegisterComponent]
public sealed partial class WorkbenchComponent : Component
{
    [DataField("allowedRecipes",
        customTypeSerializer: typeof(PrototypeIdListSerializer<ConstructionPrototype>))]
    public List<string> AllowedRecipes = new();

    [DataField("materialSearchRadius")]
    public float MaterialSearchRadius = 1.5f;
}
