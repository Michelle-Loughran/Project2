using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Entities;

public class FamilyMember
{
    public int Id { get; set; }

    // is family member the primary carer
    public bool Primary { get; set; } = false;

    // relationships

    // Foreign keys the patient related to the familymember
    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    // Foreign keys the family member of the patient
    public int MemberId { get; set; }
    [JsonIgnore]
    public User Member { get; set; }
}
