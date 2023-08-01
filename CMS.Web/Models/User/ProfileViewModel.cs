using System.ComponentModel.DataAnnotations;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;


namespace CMS.Web.Models.User;

public class ProfileViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    [Required] [StringLength(80, MinimumLength = 1)]
    public string Firstname { get; set; } = string.Empty;
    [Required] [StringLength(80, MinimumLength = 1)]
    public string Surname { get; set; } = string.Empty;
    
    public DateTime DOB { get; set; }
          
    [Required] [StringLength(80, MinimumLength = 1)]
    public string Street { get; set; } = string.Empty;
       
    [Required] [StringLength(80, MinimumLength = 1)]
    public string Town { get; set; } = string.Empty;
        
    [Required] [StringLength(80, MinimumLength = 1)]
    public string County { get; set; } = string.Empty;
        
    [Required] [StringLength(8, MinimumLength = 3)]
    public string Postcode { get; set; } = string.Empty;
       
    [Required] [StringLength(11)]
    public string MobileNumber { get; set; } = string.Empty;
     
    [Required] [StringLength(11)]
    public string HomeNumber { get; set; } = string.Empty;

    [Required] [EmailAddress] [Remote(action: "VerifyEmailAvailable", controller: "User", AdditionalFields = nameof(Id))]
    public string Email { get; set; }

    public Role Role { get; set; }

    // Properties relating to a Carer

    [StringLength(80, MinimumLength = 1)]
    public string NationalInsuranceNo { get; set; } = string.Empty;
    
    public bool DBSCheck { get; set; } = false;

    public string Qualifications { get; set; } = string.Empty;
    
    [Url]
    public string PhotoUrl { get; set; }

}
