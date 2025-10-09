
using Infrastructure.Frameworks.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Frameworks.DataBase;

public class Context : DbContext
{
    private readonly string _connectionString;
    
    public Context(string connectionString)
    {
        Console.WriteLine(Guid.NewGuid());
        _connectionString = connectionString; //options.Value.ConnectionStrings.PostgresConnection;
    }

    public Context(DbContextOptions<Context> options) : base(options)
    {
     
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }

    // Добавляем DbSet для сущностей

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    

}