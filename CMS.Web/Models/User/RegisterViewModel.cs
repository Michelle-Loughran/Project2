using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using CMS.Data.Entities;

namespace CMS.Web.Models.User;

public class RegisterViewModel
{ 
  // general attributes of all users
    [Required] [StringLength(80, MinimumLength = 1)]
    public string Firstname { get; set; } = string.Empty;

    [Required] [StringLength(80, MinimumLength = 1)]
    public string Surname { get; set; } = string.Empty;
    

    [Required]
    [EmailAddress]
    [Remote(action: "VerifyEmailAvailable", controller: "User")]
    public string Email { get; set; }

    [Required] [StringLength(80, MinimumLength = 8)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
    public string PasswordConfirm  { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role Role { get; set; }

}
