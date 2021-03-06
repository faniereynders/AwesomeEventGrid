﻿using System.Collections.Generic;

namespace AwesomeEventGrid.Abstractions.Models
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
