using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeEventGrid.Models
{
    public class TopicModel : IValidatableObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("Topic name is required", new string[] { nameof(Name) });

            }
        }
    }
}