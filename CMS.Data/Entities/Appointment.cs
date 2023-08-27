using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entities;

public class Appointment
{
  public int Id { get; set; }
  [Required]
  [StringLength(80, MinimumLength = 1)]
  public string Firstname { get; set; } = string.Empty;

  [Required]
  [StringLength(80, MinimumLength = 1)]
  public string Surname { get; set; } = string.Empty;
  public string Name => Firstname + " " + Surname;

  [Required]
  [StringLength(500, MinimumLength = 5)]
  public string Notes { get; set; }
  public DateTime Date { get; set; }
  public DateTime Time { get; set; }

  // Foreign key relating to patient and User
  public int PatientId { get; set; }
  public int UserId { get; set; }
  public Patient Patient { get; set; } // navigation property
  public User User { get; set; }
}

