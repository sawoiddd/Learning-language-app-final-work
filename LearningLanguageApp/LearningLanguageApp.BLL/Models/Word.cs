using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningLanguageApp.BLL.Models;

[Table("WORDS")]
public class Word
{
    [Required]
    [MaxLength(64)]
    public string OriginalWord { get; set; }
    [Required]
    [MaxLength(64)]
    public string Translate { get; set; }
    [Required]
    [MaxLength(32)]
    public string Type { get; set; }
    [Required]
    public bool IsLearned { get; set; }
    [Required]
    [Column(TypeName = "CHAR(2)")]
    public string Level { get; set; }
}
