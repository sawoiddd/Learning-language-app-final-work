using System.ComponentModel.DataAnnotations;

namespace LearningLanguageApp.BLL.Dtos;

public class UpdateDictionaryDto
{
    public int Id { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; }
}
