namespace ERP.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; set; }
        public DateTime? DeletadoEm { get; set; }
        public bool IsDeleted => DeletadoEm.HasValue;
    }
}
