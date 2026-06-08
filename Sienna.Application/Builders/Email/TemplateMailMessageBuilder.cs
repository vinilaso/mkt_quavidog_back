using Sienna.Application.Interfaces.Email;

namespace Sienna.Application.Builders.Email
{
    public class TemplateMailMessageBuilder<T> where T : class
    {
        private readonly MailMessageBuilder _innerBuilder;
        private readonly IMailTemplate<T> _template;

        internal TemplateMailMessageBuilder(MailMessageBuilder innerBuilder, IMailTemplate<T> template)
        {
            _innerBuilder = innerBuilder ?? throw new ArgumentNullException(nameof(innerBuilder));
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public MailMessageBuilder WithVariables(T variables)
        {
            ArgumentNullException.ThrowIfNull(variables, nameof(variables));
            return _innerBuilder.AddTemplate(_template.Id, variables);
        }
    }
}
