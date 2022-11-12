using System.ComponentModel.DataAnnotations;

namespace ServiceBusTool.ServiceBus.Models;

public class ParameterDefinition : BaseKeyValueListItem, IValidatableObject
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    public IList<Parameter> Parameters { get; set; } = new List<Parameter>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (validationContext.ObjectInstance is ParameterDefinition parameterDefinition)
        {
            var hasDuplicateParameters = parameterDefinition.Parameters
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .GroupBy(p => p.Name.ToUpperInvariant())
                .Select(g => new { g.Key, Count = g.Count() })
                .Any(p => p.Count > 1);
            if (hasDuplicateParameters)
            {
                errors.Add(new ValidationResult("There are duplicate parameters in the definition. Parameters are case-insensitive.", new []{nameof(parameterDefinition.Parameters)}));
            }
        }

        return errors;
    }
}