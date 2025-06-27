using System.Collections.Generic;

namespace LearningLanguageApp.DAL;

public class LearningLanguageAppDataContext : DbContext
{
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LearningLanguageAppDB;Integrated Security=True");
    }

    public DbSet<Word> Words { get; set; }

}
