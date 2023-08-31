using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Data.Entities;

public class AppointmentViewModel
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

  public Role Role { get; set; }
  // public DateTime DateTimeOfEvent { get; set; } = DateTime.Now;
  public SelectList Patients { set; get; }
  public SelectList Carers { set; get; }
  public SelectList Users { set; get; }

  // Foreign key relating to patient and User
  public int PatientId { get; set; }
  public int UserId { get; set; }
  public Patient Patient { get; set; } // navigation property
  public User User { get; set; }
}

