using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ServiceBusTool.ServiceBus.Models;

public class MessageDefinition : BaseKeyValueListItem, IValidatableObject
{
    private static readonly Regex ParameterRegex =
        new Regex("%(?'Parameter'\\w+)%", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    
    [Required]
    public string Body { get; set; }

    public IReadOnlyList<string> FindParameters()
    {
        var matches = ParameterRegex.Matches(Body);

        return matches
            .SelectMany<Match, Group>(m => m.Groups)
            .Where(g => g.Name == "Parameter")
            .Select(g => g.Value)
            .ToList();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();
        if (validationContext.ObjectInstance is MessageDefinition messageDefinition)
        {
            var parameters = messageDefinition.FindParameters();
            var totalCount = parameters.Count;
            var uniqueCount = parameters.DistinctBy(p => p.ToUpperInvariant()).Count();
            if (totalCount != uniqueCount)
            {
                errors.Add(new ValidationResult("There are duplicate parameters in the message body. Parameters are case-insensitive.", new []{nameof(Body)}));
            }
        }
        return errors;
    }
}