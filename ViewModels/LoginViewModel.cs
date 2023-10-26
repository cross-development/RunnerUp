using System.ComponentModel.DataAnnotations;

namespace RunnerUp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email address is required")]
    [Display(Name = "Email Address")]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}