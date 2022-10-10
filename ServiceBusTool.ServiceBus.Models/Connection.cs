using System.ComponentModel.DataAnnotations;

namespace ServiceBusTool.ServiceBus.Models;

public class Connection
{
    public Guid Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    
    [Required]
    [MinLength(10)]
    public string ConnectionString { get; set; }
}