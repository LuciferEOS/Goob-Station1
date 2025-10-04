namespace Content.Goobstation.Server.Hustlers;

[RegisterComponent]
public sealed partial class HustlerDuelComponent : Component
{
    [ViewVariables]
    public EntityUid Challenger;

    [ViewVariables]
    public EntityUid Target;
}
