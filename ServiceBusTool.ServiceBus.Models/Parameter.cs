using System.ComponentModel.DataAnnotations;

namespace ServiceBusTool.ServiceBus.Models;

public class Parameter
{
    [Required] [MinLength(3)] public string Name { get; set; } = string.Empty;

    [Required]
    public string Value { get; set; } = string.Empty;
}