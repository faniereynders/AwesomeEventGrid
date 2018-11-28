using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication23 {

    public enum HttpRequestError
    {
        Auto,
        BadRequest,
        NotFound
    }
    public class ValidateTopicAttribute : ValidationAttribute, IActionFilter
    {
        private readonly string name;
        private readonly HttpRequestError httpRequestError;

        public ValidateTopicAttribute()
        {

        }
        public ValidateTopicAttribute(string name, HttpRequestError httpRequestError = HttpRequestError.Auto)
        {
            this.name = name;
            this.httpRequestError = httpRequestError;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var topicsRepository = (ITopicsRepository)context.HttpContext.RequestServices.GetService(typeof(ITopicsRepository));
            if (!Exists(topicsRepository))
            {
                var notFoundMethods = new string[] { "GET", "PUT", "DELETE" };
                var badRequestMethods = new string[] { "POST" };
                if (httpRequestError == HttpRequestError.BadRequest || (httpRequestError == HttpRequestError.Auto && 
                    badRequestMethods.Contains(context.HttpContext.Request.Method)))
                {
                    context.Result = new BadRequestResult();
                }
                else if (httpRequestError == HttpRequestError.NotFound || (httpRequestError == HttpRequestError.Auto &&
                   notFoundMethods.Contains(context.HttpContext.Request.Method)))
                {
                    context.Result = new NotFoundResult();
                }
                
            }
        }

        private bool Exists(ITopicsRepository topicsRepository)
        {
            
            return topicsRepository.FindByName(name) != null;

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var topicsRepository = (ITopicsRepository)validationContext.GetService(typeof(ITopicsRepository));
            if (!Exists(topicsRepository))
            {
                return new ValidationResult("Topic does not exist.");
            }
            
            return ValidationResult.Success;
        }



    }
}