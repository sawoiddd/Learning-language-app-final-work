using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using LearningLanguageApp.BLL.Models;


namespace LearningLanguageApp.DAL;

public class LearningLanguageAppDataContext: DbContext
{
    private readonly string _connectionString;
    public LearningLanguageAppDataContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
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
