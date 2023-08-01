using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CMS.Data.Entities;

public class Patient

{    // primary key
    [Required]
    [Column("Patient_Id")]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 1)]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 1)]
    public string Surname { get; set; } = string.Empty;
    public string Name => Firstname + " " + Surname;

    [Required]
    [StringLength(10, MinimumLength = 1)]
    [Display(Name = "National Insurance No.")]
    public string NationalInsuranceNo { get; set; } = string.Empty;

    [Display(Name = "DOB")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DOB { get; set; }
    // readonly
    public int Age => (DateTime.Now - DOB).Days / 365;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Town { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string County { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 3)]
    public string Postcode { get; set; } = string.Empty;

    [Url]
    public string PhotoUrl { get; set; }

    [Required]
    [StringLength(11)]
    public string MobileNumber { get; set; } = string.Empty;

    [StringLength(11)]
    public string HomeNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string GP { get; set; } = string.Empty;
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string SocialWorker { get; set; } = string.Empty;

    public string CarePlan { get; set; }

    [Range(0, 10, ErrorMessage = "The number of calls should be between 1 and 10")]
    public int Calls { get; set; }
     public string Address => Street + " " + Town + " " + Postcode;

    // Relationships

    // a set of care events 
    public List<PatientCareEvent> CareEvents { get; set; }
    public List<PatientCondition> PatientConditions { get; set; }
    public List<FamilyMember> FamilyMembers { get; set; }
   
}
