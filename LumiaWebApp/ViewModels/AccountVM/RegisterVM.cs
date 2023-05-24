using System.ComponentModel.DataAnnotations;

namespace LumiaWebApp.ViewModels.AccountVM
{
    public class RegisterVM
    {
        [Required,MaxLength(15)]
        public string Username { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        [DataType(DataType.Password), MinLength(8) ,Compare(nameof(Password))]
        public string ConfrimPassword { get; set; } = null!;
    }
}
