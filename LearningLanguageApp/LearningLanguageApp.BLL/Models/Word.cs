using LearningLanguageApp.BLL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningLanguageApp.BLL.Models;

[Table("WORDS")]
public class Word
{
    public int Id { get; set; }
    [Required]
    [MaxLength(64)]
    public string OriginalWord { get; set; }
    [Required]
    [MaxLength(64)]
    public string Translation { get; set; }
    [Required]
    public WordTypeEnum Type { get; set; }
    [Required]
    public bool IsLearned { get; set; }
    [Required]
    public WordLevelEnum Level { get; set; }
    public int DictionaryID { get; set; }
    [ForeignKey(nameof(DictionaryID))]
    public Dictionary Dictionary { get; set; }
}
