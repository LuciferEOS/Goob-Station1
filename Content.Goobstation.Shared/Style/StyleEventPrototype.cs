using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Style
{
    [Prototype("styleEvent")]
    public sealed class StyleEventPrototype : IPrototype
    {
        [IdDataField] public string ID { get; } = default!;

        [DataField]
        public float Points;

        [DataField]
        public string Description = string.Empty;
    }
}
