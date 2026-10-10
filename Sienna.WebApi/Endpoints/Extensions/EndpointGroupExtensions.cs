namespace Sienna.WebApi.Endpoints.Extensions
{
    internal static class EndpointGroupExtensions
    {
        internal static IEndpointRouteBuilder MapEndpointGroups(this IEndpointRouteBuilder builder)
        {
            var groups = from type in typeof(IEndpointGroup).Assembly.GetTypes()
                         where type is { IsClass: true, IsAbstract: false } && type.IsAssignableTo(typeof(IEndpointGroup))
                         select (IEndpointGroup)Activator.CreateInstance(type)!;

            foreach (var group in groups)
                group.Map(builder);

            return builder;
        }
    }
}
