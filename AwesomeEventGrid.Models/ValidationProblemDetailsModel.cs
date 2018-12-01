using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeEventGrid.Models
{
    public class ValidationProblemDetailsModel: ProblemDetailsModel
    {
        public ValidationProblemDetailsModel()
        {

        }
        public ValidationProblemDetailsModel(ModelState modelState)
        {
            Errors = modelState;
        }
        public IReadOnlyDictionary<string,string[]> Errors { get; }
    }
}
