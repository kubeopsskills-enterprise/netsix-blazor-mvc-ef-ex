using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KubeExSite.Context.Models;

public class RoutineReport
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Topic { get; set; }
    public string Body { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}