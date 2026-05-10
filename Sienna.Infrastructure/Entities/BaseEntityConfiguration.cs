using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Infrastructure.Database;
using Sienna.Shared.Extensions.System;

namespace Sienna.Infrastructure.Entities
{
    internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        protected abstract ModulePrefix Module { get; }
        protected abstract string TableName { get; }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(GetTableName());

            ConfigureSpecific(builder);
        }

        private string GetTableName()
        {
            return $"{Module.Value}_{TableName.ToSnakeCaseUpper()}";
        }

        protected abstract void ConfigureSpecific(EntityTypeBuilder<T> builder);
    }
}
