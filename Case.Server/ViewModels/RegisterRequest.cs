using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Case.Server.ViewModels;


public class RegisterViewModel
{
    [Column("Ad Soyad")]
    [Required]
    [StringLength(64)]
    public required string FullName { get; set; }


    [Column("Eposta")]
    [Required]
    [StringLength(256)]
    public required string Email { get; set; }


    [Column("Şifre")]
    [Required]
    [StringLength(256)]
    public required string Password { get; set; }
}