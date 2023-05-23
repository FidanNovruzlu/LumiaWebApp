using System.ComponentModel.DataAnnotations;

namespace LumiaWebApp.ViewModels.AccountVM
{
    public class LoginVM
    {
        public string Email { get; set; } = null!;
        [DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = null!;
    }
}
