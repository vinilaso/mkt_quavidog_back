using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Sienna.WebApi.Middlewares
{
    internal sealed class QueryEnumNormalizationMiddleware(RequestDelegate next)
    {
        private sealed record QueryEnumParameter(string Name, Type EnumType);

        private static readonly ConcurrentDictionary<Endpoint, IReadOnlyList<QueryEnumParameter>> Cache = new();

        public async Task InvokeAsync(HttpContext context)
        {
            var enpoint = context.GetEndpoint();

            if (enpoint is null || context.Request.Query.Count == 0)
            {
                await next(context);
                return;
            }

            var enumParameters = Cache.GetOrAdd(enpoint, Discover);

            if (enumParameters.Count == 0)
            {
                await next(context);
                return;
            }

            var query = new Dictionary<string, StringValues>(context.Request.Query, StringComparer.OrdinalIgnoreCase);

            foreach (var parameter in enumParameters)
            {
                if (!query.TryGetValue(parameter.Name, out var values))
                    continue;

                var normalized = new string?[values.Count];

                for (var i = 0; i < values.Count; i++)
                {
                    if (!TryNormalize(parameter.EnumType, values[i], out var enumName))
                    {
                        await InvalidValue(parameter, values[i]).ExecuteAsync(context);
                        return;
                    }

                    normalized[i] = enumName;
                }

                query[parameter.Name] = new StringValues(normalized);
            }

            context.Request.Query = new QueryCollection(query);
            await next(context);
        }

        private static bool TryNormalize(Type enumType, string? value, out string? enumName)
        {
            enumName = null;
            var trimmed = value?.Trim() ?? string.Empty;

            if (trimmed.Length == 0 || !char.IsLetter(trimmed[0]))
                return false;

            if (!Enum.TryParse(enumType, trimmed, ignoreCase: true, out var parsed) || !Enum.IsDefined(enumType, parsed))
                return false;

            enumName = parsed.ToString();
            return true;
        }

        private static ProblemHttpResult InvalidValue(QueryEnumParameter parameter, string? value)
        {
            var accepted = string.Join(", ", Enum.GetNames(parameter.EnumType).Select(JsonNamingPolicy.CamelCase.ConvertName));

            return TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Dados inválidos",
                type: "Query.InvalidValue",
                detail: $"O valor \"{value}\" não é válido para o parâmetro {JsonNamingPolicy.CamelCase.ConvertName(parameter.Name)}. Valores aceitos: {accepted}.");
        }

        private static IReadOnlyList<QueryEnumParameter> Discover(Endpoint endpoint)
        {
            if (endpoint.Metadata.GetMetadata<MethodInfo>() is not MethodInfo handler)
                return [];

            List<QueryEnumParameter> found = [];

            foreach (var parameter in handler.GetParameters())
            {
                if (parameter.GetCustomAttribute<AsParametersAttribute>() is not null)
                {
                    var constructor = parameter.ParameterType.GetConstructors().MaxBy(c => c.GetParameters().Length);

                    foreach (var property in constructor?.GetParameters() ?? [])
                        TryAdd(found, property);
                }
                else
                {
                    TryAdd(found, parameter);
                }
            }

            return found;
        }

        private static void TryAdd(List<QueryEnumParameter> found, ParameterInfo parameter)
        {
            var isFromOtherSource =
                parameter.GetCustomAttribute<FromRouteAttribute>() is not null ||
                parameter.GetCustomAttribute<FromBodyAttribute>() is not null ||
                parameter.GetCustomAttribute<FromHeaderAttribute>() is not null ||
                parameter.GetCustomAttribute<FromFormAttribute>() is not null;

            if (isFromOtherSource)
                return;

            var type = parameter.ParameterType;

            if (type is null)
                return;

            type = type.IsArray ? type.GetElementType() : type;

            if (type is null)
                return;

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (!type.IsEnum)
                return;

            var name = parameter.GetCustomAttribute<FromQueryAttribute>()?.Name ?? parameter.Name!;
            found.Add(new QueryEnumParameter(name, type));
        }
    }
}
