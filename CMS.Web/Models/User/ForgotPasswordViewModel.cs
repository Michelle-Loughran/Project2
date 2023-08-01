using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Models.User;
public class ForgotPasswordViewModel
{
    [Required]
    public string Email { get; set; }
    
}
