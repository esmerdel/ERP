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

        // DbSets
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<VendaProduto> VendaProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Mapeamento padrão do IsDeleted (BaseEntity)
            modelBuilder.Entity<Empresa>(entity =>
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

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.IsDeleted)
                      .HasColumnType("tinyint(1)")
                      .HasDefaultValue(false);
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(e => e.IsDeleted)
                      .HasColumnType("tinyint(1)")
                      .HasDefaultValue(false);

                entity.Property(p => p.Preco)
                      .HasColumnType("decimal(10,2)");
            });

            // 🔹 Venda
            modelBuilder.Entity<Venda>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.ValorTotal)
                      .HasColumnType("decimal(10,2)");

                entity.HasMany(v => v.Itens)
                      .WithOne()
                      .HasForeignKey("VendaId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 🔹 VendaProduto (entidade de junção)
            modelBuilder.Entity<VendaProduto>(entity =>
            {
                entity.HasKey("VendaId", "ProdutoId");

                entity.Property(vp => vp.PrecoUnitario)
                      .HasColumnType("decimal(10,2)");

                entity.Property(vp => vp.Subtotal)
                      .HasColumnType("decimal(10,2)");
            });

            // 🔹 Filtro global: IsDeleted == false && EmpresaId == _empresaId
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");

                    var isDeletedProp = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var notDeleted = Expression.Equal(isDeletedProp, Expression.Constant(false));

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
