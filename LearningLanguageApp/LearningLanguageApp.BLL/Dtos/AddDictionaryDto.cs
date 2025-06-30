namespace LearningLanguageApp.BLL.Dtos;

public class AddDictionaryDto
{
    public int UserId { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; }
}
