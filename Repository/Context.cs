
using Infrastructure.Frameworks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ConfigureBaseModel(modelBuilder.Entity<Products>());
        // ConfigureBaseModel(modelBuilder.Entity<User>());
        // ConfigureBaseModel(modelBuilder.Entity<Guest>());
        // ConfigureBaseModel(modelBuilder.Entity<Basket>());
        // ConfigureBaseModel(modelBuilder.Entity<BoughtUserProduct>());
        // ConfigureBaseModel(modelBuilder.Entity<Role>());
        // ConfigureBaseModel(modelBuilder.Entity<DicProductsType>());
        
        modelBuilder.Entity<Products>().ToTable("Products");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Guest>().ToTable("Guest");
        modelBuilder.Entity<BoughtUserProduct>().ToTable("BoughtUserProduct");
        modelBuilder.Entity<Basket>().ToTable("Basket");
        modelBuilder.Entity<Role>().ToTable("Role");
        modelBuilder.Entity<UserRole>().ToTable("UserRole");
        modelBuilder.Entity<DicProductsType>().ToTable("DicProductsType");
    }

    private void ConfigureBaseModel<TEntity>(EntityTypeBuilder<TEntity> entity)
        where TEntity : BaseModel
    {
        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        entity.Property(e => e.CreateAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UpdateAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
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
    public DbSet<Guest> Guest { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<BoughtUserProduct> BoughtUserProduct { get; set; }
    public DbSet<Basket> Basket { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<DicProductsType> DicProductsType { get; set; }

    

}