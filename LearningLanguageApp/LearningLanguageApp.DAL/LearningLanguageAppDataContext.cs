using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using LearningLanguageApp.BLL.Models;


namespace LearningLanguageApp.DAL;

public class LearningLanguageAppDataContext: DbContext
{
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LearningLanguageAppDB;Integrated Security=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>()
            .Property(w => w.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Word>()
            .Property(w => w.Level)
            .HasConversion<string>();      
    }
    public DbSet<Word> Words { get; set; }
    public DbSet<User> Users { get; set; }


}
