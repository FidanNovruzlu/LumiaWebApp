﻿using System.ComponentModel.DataAnnotations;

namespace LumiaWebApp.ViewModels.AccountVM
{
    public class LoginVM
    {
        [Required,MaxLength(15)]
        public string UserName { get; set; } = null!;
        [DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = null!;
    }
}
