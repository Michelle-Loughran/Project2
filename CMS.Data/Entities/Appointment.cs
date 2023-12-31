using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CMS.Data.Entities;

public class Appointment
{
  public int Id { get; set; }
  [Required]
  [StringLength(80, MinimumLength = 1)]
  public string PatientFirstname { get; set; } = string.Empty;

  [Required]
  [StringLength(80, MinimumLength = 1)]
  public string PatientSurname { get; set; } = string.Empty;
  public string PatientName => PatientFirstname + " " + PatientSurname;
  [Required]
  [StringLength(80, MinimumLength = 1)]
  public string CarerFirstname { get; set; } = string.Empty;
    [Required]
  [StringLength(80, MinimumLength = 1)]
  public string CarerSurname { get; set; } = string.Empty;
  public string CarerName => CarerFirstname + " " + CarerSurname;
  public DateTime Date { get; set; }
  public DateTime Time { get; set; }
  // public DateTime DateTimeOfEvent { get; set; } = DateTime.Now;
  public DateTime DateTimeCompleted { get; set; } = DateTime.MaxValue;

  // Foreign key relating to patient and User
  public int PatientId { get; set; }
  public int UserId { get; set; }
  public Role Role { get; set; }
  [JsonIgnore]
  public Patient Patient { get; set; } // navigation property
  public User User { get; set; }

}

