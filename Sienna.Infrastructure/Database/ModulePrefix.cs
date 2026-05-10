namespace Sienna.Infrastructure.Database
{
    public sealed class ModulePrefix
    {
        public static readonly ModulePrefix Identity = new("IDENTITY");
        public static readonly ModulePrefix Workflow = new("WORKFLOW");

        public string Value { get; }

        private ModulePrefix(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;

        public static implicit operator string(ModulePrefix prefix) => prefix?.Value ?? string.Empty;
    }
}
