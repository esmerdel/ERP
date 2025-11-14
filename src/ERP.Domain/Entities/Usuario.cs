namespace ERP.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }  // a senha será armazenada com hash
        public string Role { get; set; } = "User"; // papel: Admin, User, etc.

        // FK -> referência para Empresa
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
