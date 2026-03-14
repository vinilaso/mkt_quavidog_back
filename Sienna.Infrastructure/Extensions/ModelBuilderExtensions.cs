using Microsoft.EntityFrameworkCore;
using Sienna.Shared.Extensions.System;

namespace Sienna.Infrastructure.Extensions
{
    internal static class ModelBuilderExtensions
    {
        internal static void ApplySnakeCaseUpperConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToSnakeCaseUpper());
                }
            }
        }
    }
}
