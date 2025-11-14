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

        // Construtor do DbContext, recebe as configurações do banco
        public ERPContext(DbContextOptions<ERPContext> options) : base(options) { }

        // Cada DbSet vira uma tabela no banco de dados
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Método executado quando o EF está construindo o modelo do banco
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mantém a configuração padrão do Entity Framework
            base.OnModelCreating(modelBuilder);

            // Percorre TODAS as entidades mapeadas pelo EF (Empresa, Usuario etc.)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Se a entidade herda de BaseEntity, aplicamos filtros globais
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // "e" será o parâmetro usado na expressão do filtro (ex: e => ...)
                    var parameter = Expression.Parameter(entityType.ClrType, "e");

                    // Acessa a propriedade "IsDeleted" da entidade: e.IsDeleted
                    var isDeletedProp = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));

                    // Cria a expressão: !e.IsDeleted  (ou seja, registro NÃO deletado)
                    var notDeleted = Expression.Not(isDeletedProp);

                    // Acessa a propriedade "EmpresaId": e.EmpresaId
                    var empresaProp = Expression.Property(parameter, nameof(BaseEntity.EmpresaId));

                    // Valor fixo da empresa atual: 1 (mock)
                    var empresaIdValue = Expression.Constant(_empresaId);

                    // Cria a expressão: e.EmpresaId == 1
                    var sameEmpresa = Expression.Equal(empresaProp, empresaIdValue);

                    // Combina as duas condições: 
                    // (!e.IsDeleted) AND (e.EmpresaId == 1)
                    var combined = Expression.AndAlso(notDeleted, sameEmpresa);

                    // Converte essa expressão em um lambda: e => !e.IsDeleted && e.EmpresaId == 1
                    var lambda = Expression.Lambda(combined, parameter);

                    // Aplica o filtro global à entidade
                    // Agora todas as consultas já virão filtradas automaticamente
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
