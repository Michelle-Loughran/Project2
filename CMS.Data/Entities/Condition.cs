using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entities;

public class Condition
{
    public int Id { get; set; }
    [Required][StringLength(80, MinimumLength = 1)]
    public string Name { get; set; }   

    [Required][StringLength(1000, MinimumLength = 1)]
    public string Description { get; set; }


}

