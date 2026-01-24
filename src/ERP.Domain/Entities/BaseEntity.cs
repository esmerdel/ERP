namespace ERP.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // Empresa que "possui" o registro
        public int EmpresaId { get; set; }

        // Auditoria
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
        public DateTime? AtualizadoEm { get; private set; }
        public DateTime? DeletadoEm { get; private set; }

        //Exclusão lógica
        public bool IsDeleted { get; private set; }

        //Métodos utilitários de ciclo de vida
        public void MarcarComoCriado()
        {
            CriadoEm = DateTime.UtcNow;
            IsDeleted = false;
        }

        public void MarcarComoAtualizado()
        {
            AtualizadoEm = DateTime.UtcNow;
        }

        public void MarcarComoDeletado()
        {
            DeletadoEm = DateTime.UtcNow;
            IsDeleted = true;
        }
    }
}
