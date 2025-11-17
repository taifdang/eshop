using Infrastructure.Identity.Models;

namespace Infrastructure.Identity.Data.Seed;

public static class InitialData
{
    public static List<ApplicationUser> Users { get; set; }

    static InitialData()
    {
        Users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "peter",
                Email = "peter@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "mira",
                Email = "mira@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
        };
    }
}
