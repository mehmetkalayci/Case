using Case.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Case.Server.Data;


public static class SeedData
{
    public static void Seed(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();

        context!.Database.Migrate();

        if (context.Users.Count() == 0)
        {
            context.Users.AddRange(
                new List<User>() {
                         new User() { FullName="Admin", Email="admin@aa.com", Password = "123", Role = AuthRoles.Admin },
                         new User() { FullName="Personel", Email="personel@aa.com", Password = "123", Role = AuthRoles.Personel },
                         new User() { FullName="Kullanıcı", Email="kullanici@aa.com", Password = "123", Role = AuthRoles.Kullanici },
                });
        }

        context.SaveChanges();

    }

}
