using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Entities;


public class PatientCondition
{
    public int Id { get; set; }

    public DateTime DateTimeConditionAdded { get; set; }

    // notes on the condition for this patient
    public string Note { get; set; }

    // relationship

    // a patient condition 
    public int ConditionId { get; set; }
    public Condition Condition { get; set; }

    // patient having the condition
    public int PatientId { get; set; }
    public Patient Patient { get; set; }


}
