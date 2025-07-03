using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningLanguageApp.BLL.Models;

[Table("USERS")]
public class User
{
    public int Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(32)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(32)]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public DateTime BirthdayDate { get; set; }
    [Required]
    public DateTime CreateAt { get; set; }
    [Required]
    public DateTime UpdateAt{ get; set; }
    public ICollection<Dictionary> Dictionaries { get; set; }
}
