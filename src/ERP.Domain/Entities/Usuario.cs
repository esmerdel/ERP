namespace ERP.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string Role { get; set; } = "Funcionario"; // Padrão
    }
}
