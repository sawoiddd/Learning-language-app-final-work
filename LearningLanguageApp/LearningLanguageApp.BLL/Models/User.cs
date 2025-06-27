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
    [MaxLength(32)]
    [Required]
    public string LastName { get; set; }
    [MaxLength(32)]
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public DateTime Bdate { get; set; }
    [Required]
    public DateTime CreateDate { get; set; }
    [Required]
    public DateTime UpdateDate { get; set; }
}
