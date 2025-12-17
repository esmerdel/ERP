using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Infrastructure.Context
{
    public class ERPContext : DbContext
    {
        // Mock temporário: identifica a empresa atual  
        // (no futuro virá do token JWT)
        private readonly int _empresaId = 1;
        public ERPContext(DbContextOptions<ERPContext> options) : base(options) { }


        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var isDeletedProp = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var notDeleted = Expression.Not(isDeletedProp);
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
