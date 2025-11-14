namespace ERP.API.ViewModels.Auth
{
    public class RegisterRequest
    {
        public string NomeEmpresa { get; set; }
        public string Cnpj { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
