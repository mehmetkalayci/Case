using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Case.Server.ViewModels;

public class LoginRequest
{
    [Column("Eposta")]
    [Required(ErrorMessage = "Eposta adresi girin")]
    public required string Email { get; set; }

    [Column("Şifre")]
    [Required(ErrorMessage = "Şifrenizi girin")]
    public required string Password { get; set; }
}
