using System.ComponentModel.DataAnnotations.Schema;

namespace Case.Server.ViewModels;


public class LoggedInUserVM
{
    [Column("Id")]
    public int UserId { get; set; }

    [Column("Ad Soyad")]
    public string FullName { get; set; }

    [Column("Eposta")]
    public string Email { get; set; }

    [Column("Rol")]
    public string Role { get; set; }
}
