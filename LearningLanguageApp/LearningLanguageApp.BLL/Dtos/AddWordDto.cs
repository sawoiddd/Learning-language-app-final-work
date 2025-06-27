using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningLanguageApp.BLL.Dtos;

public class AddWordDto
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
    [Column(TypeName = "CHAR(2)")]
    public string Level { get; set; }
}
