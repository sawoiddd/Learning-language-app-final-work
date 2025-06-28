using LearningLanguageApp.BLL.Enums;

namespace LearningLanguageApp.BLL.Dtos;

public class AddWordDto
{
    public string OriginalWord { get; set; }
    public string Translation { get; set; }
    public WordTypeEnum Type { get; set; }
    public WordLevelEnum Level { get; set; }
}
