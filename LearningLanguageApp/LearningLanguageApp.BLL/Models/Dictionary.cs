using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningLanguageApp.BLL.Models;

[Table("DICTIONARIES")]
public class Dictionary
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    [Required]
    [MaxLength(32)]
    public string SourceLanguage { get; set; }
    [Required]
    [MaxLength(32)]
    public string TargetLanguage { get; set; }
    public ICollection<Word> Words { get; set; }
}
