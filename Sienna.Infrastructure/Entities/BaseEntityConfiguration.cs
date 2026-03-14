using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities;
using Sienna.Infrastructure.Database;
using Sienna.Shared.Extensions.System;

namespace Sienna.Infrastructure.Entities
{
    internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IBaseEntity
    {
        protected abstract ModulePrefix Module { get; }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(GetTableName());

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .ValueGeneratedNever()
                .IsRequired();

            ConfigureSpecific(builder);
        }

        private string GetTableName()
        {
            string className = typeof(T).Name;
            return $"{Module.Value}_{className.ToSnakeCaseUpper()}";
        }

        protected abstract void ConfigureSpecific(EntityTypeBuilder<T> builder);
    }
}
