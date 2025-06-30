namespace LearningLanguageApp.BLL.Dtos;

public class RegisterUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime BirthdayDate { get; set; }
}
