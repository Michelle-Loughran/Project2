using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Data.Validators;

namespace CMS.Data.Entities;

public class PatientCareEvent
{
    public int Id { get; set; }

    [DisplayName("Scheduled For")]
    public DateTime DateTimeOfEvent { get; set; } = DateTime.Now;

    // [Range(8,60)]
    public int ScheduledDuration { get; set; } = 30;

    [DateGreaterThan("DateTimeOfEvent")]
    [DisplayName("Completed On")]
    public DateTime DateTimeCompleted { get; set; } = DateTime.MaxValue;

    [Range(8, 60)]
    public int ActualDuration { get; set; } = 30;

    // copy of Patient.CarePlan made when Event created
    public string CarePlan { get; set; }


    // Notes documenting completion of Careplan
    public string Issues { get; set; }

    // relationships

    // the patient the care event is performed on
    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    // the carer who performed the care event
    public int UserId { get; set; }
    public User User { get; set; }


}

