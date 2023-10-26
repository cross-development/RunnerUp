using System.ComponentModel.DataAnnotations;

namespace RunnerUp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email address is required")]
    [Display(Name = "Email Address")]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}