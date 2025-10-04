using Robust.Shared.GameStates;

namespace Content.Goobstation.Server.Hustlers;

[RegisterComponent]
public sealed partial class HustlerChallengePendingComponent : Component
{
    [DataField]
    public EntityUid Challenger;

    [DataField]
    public TimeSpan ExpiryTime;
}
