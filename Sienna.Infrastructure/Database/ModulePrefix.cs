namespace Sienna.Infrastructure.Database
{
    public sealed class ModulePrefix
    {
        public static readonly ModulePrefix Identity = new("IDENTITY");
        public static readonly ModulePrefix Workflow = new("WORKFLOW");
        public static readonly ModulePrefix Media = new("MEDIA");
        public static readonly ModulePrefix Social = new("SOCIAL");

        public string Value { get; }

        private ModulePrefix(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;

        public static implicit operator string(ModulePrefix prefix) => prefix?.Value ?? string.Empty;
    }
}
