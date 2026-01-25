public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public int EmpresaId { get; protected set; }

    public DateTime CriadoEm { get; protected set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; protected set; }
    public DateTime? DeletadoEm { get; protected set; }

    public bool IsDeleted { get; protected set; }

    public void MarcarComoDeletado()
    {
        IsDeleted = true;
        DeletadoEm = DateTime.UtcNow;
    }

    public void Atualizar()
    {
        AtualizadoEm = DateTime.UtcNow;
    }
}
