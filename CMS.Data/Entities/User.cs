using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Entities;

// Add User roles relevant to your application
public enum Role { admin, manager, carer, family, guest }

public class User
{
    public int Id { get; set; }

    // general attributes of all users
    public string Title { get; set; } = string.Empty;

    [Required] [StringLength(80, MinimumLength = 1)]
    public string Firstname { get; set; } = string.Empty;

    [Required] [StringLength(80, MinimumLength = 1)]
    public string Surname { get; set; } = string.Empty;
    
    public DateTime DOB { get; set; }
    
    [Required] [StringLength(80, MinimumLength = 1)]
    public string Email { get; set; } = string.Empty;
    
    [Required] [StringLength(80, MinimumLength = 1)]
    public string Street { get; set; } = string.Empty;
    
    [Required]  [StringLength(80, MinimumLength = 1)]
    public string Town { get; set; } = string.Empty;
    
    [Required] [StringLength(80, MinimumLength = 1)]
    public string County { get; set; } = string.Empty;
    
    [Required] [StringLength(8, MinimumLength = 3)]
    public string Postcode { get; set; } = string.Empty;
    
    [Required] [StringLength(11)]
    public string MobileNumber { get; set; } = string.Empty;
    
    [StringLength(11)]
    public string HomeNumber { get; set; } = string.Empty;
    
    public string Password { get; set; }       
    public Role Role { get; set; }
    public int UserId { get; set; }

    public string Name => Firstname + " " + Surname;
    public int Age => (DateTime.Now - DOB).Days / 365;


    // Properties relating to a Carer

    [StringLength(80, MinimumLength = 1)]
    public string NationalInsuranceNo { get; set; } = string.Empty;
    
    public bool DBSCheck { get; set; } = false;

    public string Qualifications { get; set; } = string.Empty;
    
    [Url]
    public string PhotoUrl { get; set; }
  
    //public List<Appointment> Appointments { get; set; }

}

