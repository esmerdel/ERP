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
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Garante que o campo IsDeleted seja mapeado corretamente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.IsDeleted)
                      .HasColumnType("tinyint(1)")
                      .HasDefaultValue(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.IsDeleted)
                      .HasColumnType("tinyint(1)")
                      .HasDefaultValue(false);
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.Property(e => e.IsDeleted)
                      .HasColumnType("tinyint(1)")
                      .HasDefaultValue(false);
            });

            // 🔹 Mantém o filtro global (IsDeleted == false && EmpresaId == _empresaId)
            // 🔹 Mantém o filtro global (IsDeleted == false && EmpresaId == _empresaId)
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

                    // 💡 Remova o HasField — agora o EF cria automaticamente o backing field
                }
            }


        }
    }
}