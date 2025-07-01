using LearningLanguageApp.BLL.Enums;

namespace LearningLanguageApp.BLL.Dtos;

public class UpdateWordDto
{
    public int Id { get; set; }
    public string OriginalWord { get; set; }
    public string Translation { get; set; }
    public WordTypeEnum Type { get; set; }
    public WordLevelEnum Level { get; set; }
}
