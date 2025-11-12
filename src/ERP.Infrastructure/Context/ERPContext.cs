using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Infrastructure.Context
{
    public class ERPContext : DbContext
    {
        private readonly int _empresaId = 1; // mock por enquanto (será pego do token depois)

        public ERPContext(DbContextOptions<ERPContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtro global: exclui registros deletados e filtra por EmpresaId
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");

                    // e => !e.IsDeleted
                    var isDeletedProp = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var notDeleted = Expression.Not(isDeletedProp);

                    // e => e.EmpresaId == _empresaId
                    var empresaProp = Expression.Property(parameter, nameof(BaseEntity.EmpresaId));
                    var empresaIdValue = Expression.Constant(_empresaId);
                    var sameEmpresa = Expression.Equal(empresaProp, empresaIdValue);

                    var combined = Expression.AndAlso(notDeleted, sameEmpresa);
                    var lambda = Expression.Lambda(combined, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
