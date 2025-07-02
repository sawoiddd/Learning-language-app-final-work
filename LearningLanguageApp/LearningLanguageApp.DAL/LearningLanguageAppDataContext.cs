using Humanizer;
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
    public LearningLanguageAppDataContext(DbContextOptions<LearningLanguageAppDataContext> options)
    : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>()
            .Property(w => w.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Word>()
            .Property(w => w.Level)
            .HasConversion<string>();
        modelBuilder.Entity<User>()
           .Property(w => w.Login)
           .IsUnicode();
    }
    public DbSet<Word> Words { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Dictionary> Dictionaries { get; set; } 

}
