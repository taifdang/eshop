using IdentityService.Infrastructure.Entity;

namespace IdentityService.Infrastructure.Seed;

public static class InitialData
{
    public static List<User> Users { get; set; }

    static InitialData()
    {
        Users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "peter",
                Email = "peter@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "mira",
                Email = "mira@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
        };
    }
}


